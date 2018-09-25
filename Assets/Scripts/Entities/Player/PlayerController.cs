using mytest.Use;
using System.Collections;
using mytest.UI.InputSystem;
using UnityEngine;
using MalbersAnimations;

[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [Header(" - Move")]
    public float MoveSpeed = 3;
    public float RotateSpeed = 5;
    [Header(" - Jump")]
    public float JumpSpeed = 3.0f;
    public float Gravity = 9.8f;

    //Objects
    private CollisionController m_CollisionController;
    private CharacterController m_CharacterController;
    private PlayerAnimationController m_PlayerAnimationController;
    private Item_Base m_Assistant;
    private WeaponController m_Weapon;
    private IEnumerator m_ReduceSpeedCoroutine;

    //Values
    private Vector3 m_MoveDir = Vector3.zero;
    private Vector3 m_LastActiveMoveDir;
    private Vector3 m_MoveDirAtLockInput;
    private Quaternion m_TargetRot;
    private float m_CurVerticalSpeed = 0;   //Текущая вертикальная скорость
    private bool m_IsInAir = false;         //Находиться ли игрок в воздухе         
    private bool m_JumpLastFrame = false;   //Совершен ли пріжок в последнем кадре

    public const float ReduceSpeedAtLockInputTime = 0.5f;

    public Vector3 MoveDir
    {
        get { return m_MoveDir; }
    }

    void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CollisionController = GetComponent<PlayerCollisionController>();
        m_PlayerAnimationController = GetComponent<PlayerAnimationController>();
    }

    void Start()
    {
        m_TargetRot = transform.rotation;

        //Подписаться на события
#if UNITY_EDITOR  
        InputManager.Instance.KeyboardInput.OnJump += Jump;

        if (InputManager.Instance.PreferVirtualJoystickInEditor)
            InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
        else
            InputManager.Instance.KeyboardInput.OnMove += MoveInDir;

#else
        InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
#endif

        InputManager.Instance.OnInputStateChange += InputStatusChangeHandler;
    }

    void Update()
    {
        //Горизонтальное направление зависит от скорости передвижения
        Vector3 moveSpeed = m_MoveDir * MoveSpeed;               

        //Если прыгали и приземлились (m_CharacterController.isGrounded отслеживает приземление)
        if (m_IsInAir && m_CharacterController.isGrounded)
            m_IsInAir = false;

        //Если на змемле - разрешить вращение
        if (!m_IsInAir) 
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRot, Time.deltaTime * RotateSpeed);
        }
        //Если в воздухе - дейтсвие гравитации
        else
        {
            m_CurVerticalSpeed -= Gravity * Time.deltaTime;
            moveSpeed.y = m_CurVerticalSpeed;  //Вертикальная скорость не зависит от скорости передвижения  
        }

        Debug.Log(moveSpeed);
        m_CharacterController.Move(moveSpeed * Time.deltaTime);
    }

    void LateUpdate()
    {
        //Move animation
        m_PlayerAnimationController.PlayMoveAnimation(new Vector2(m_MoveDir.x, m_MoveDir.z).magnitude, 
                                                      m_LastActiveMoveDir, 
                                                      m_TargetRot, 
                                                      m_IsInAir,
                                                      m_JumpLastFrame);

        //Если игрок в воздухе и на предыдущем кадре совершен прыжок
        if (m_IsInAir && m_JumpLastFrame)
            m_JumpLastFrame = false;
    }

    public void MoveInDir(Vector3 dir)
    {
        m_MoveDir = dir;

        if (m_MoveDir != Vector3.zero)
        {
            //Rotate in move dir
            float angle = Mathf.Atan2(m_MoveDir.x, m_MoveDir.z) * Mathf.Rad2Deg;
            m_TargetRot = Quaternion.AngleAxis(angle, Vector3.up);

            m_LastActiveMoveDir = m_MoveDir;
        }
    }

    public void Jump()
    {
        if (m_IsInAir)
            return;

        m_CurVerticalSpeed = JumpSpeed;
        m_JumpLastFrame = true;
        m_IsInAir = true;
    }

    public void PauseAnimations(bool isPaused)
    {
        m_PlayerAnimationController.PauseAnimations(isPaused);
    }

    public void DestroyPlayer()
    {
        InputManager.Instance.VirtualJoystickInput.OnMove -= MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove -= MoveInDir;

        m_PlayerAnimationController.PlayMoveAnimation(0, Vector3.zero, Quaternion.identity);
        m_PlayerAnimationController.enabled = false;
        m_CollisionController.enabled = false;
        enabled = false;
    }


	void InputStatusChangeHandler(bool state)
	{
        if (!state)
        {
            m_MoveDirAtLockInput = m_MoveDir;

            if (m_ReduceSpeedCoroutine != null)
                StopCoroutine(m_ReduceSpeedCoroutine);

            m_ReduceSpeedCoroutine = SmoothReduceMoveSpeed();

            StartCoroutine(m_ReduceSpeedCoroutine);
        }
	}

	IEnumerator SmoothReduceMoveSpeed()
	{
        float speed = 1.0f / ReduceSpeedAtLockInputTime;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
		{
            m_MoveDir = Vector3.Lerp(m_MoveDirAtLockInput, Vector3.zero, t);
            m_PlayerAnimationController.PlayMoveAnimation(m_MoveDir.magnitude, m_MoveDir, m_TargetRot);
			yield return null;
        }

		m_MoveDir = Vector3.zero;
        m_PlayerAnimationController.StopPlayerMoveAnimation();

        m_ReduceSpeedCoroutine = null;
	}


	public bool TryAddAssistant(Item_Base assistant)
    {
        if (m_Assistant != null)
            return false;

        m_Assistant = assistant;

        return true;
    }

    public bool TryAddWeapon(Item_Base weapon)
    {
        if (m_Weapon != null)
            return false;

        m_Weapon = (WeaponController)weapon;

        return true;
    }

    public void UseAssistant()
    {
        if (m_Assistant != null)
        {
            m_Assistant.Use();
            m_Assistant = null;
        }
    }

    public void UseWeapon()
    {
        if (m_Weapon != null)
        {
            m_Weapon.Use();
            m_Weapon = null;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        m_CollisionController.HandleEnterCollision(other); 
    }

    void OnTriggerExit(Collider other)
    {
        m_CollisionController.HandlerExitCollision(other);
	}
}
