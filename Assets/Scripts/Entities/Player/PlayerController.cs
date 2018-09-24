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
    public float MoveSpeed = 3;
    public float RotateSpeed = 5;

    protected Vector3 m_MoveDir = Vector3.zero;
    private CollisionController m_CollisionController;
    private CharacterController m_CharacterController;
    private PlayerAnimationController m_PlayerAnimationController;

    private Item_Base m_Assistant;
    private WeaponController m_Weapon;
    private Quaternion m_TargetRot;

    private Vector3 m_LastActiveMoveDir;
    private Vector3 m_MoveDirAtLockInput;
    private IEnumerator m_ReduceSpeedCoroutine;

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
        InputManager.Instance.KeyboardInput.OnMove += MoveInDir;
        InputManager.Instance.KeyboardInput.OnJump += Jump;

        if (InputManager.Instance.PreferVirtualJoystickInEditor)
            InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
#else
        InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
#endif

        InputManager.Instance.OnInputStateChange += InputStatusChangeHandler;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetRot, Time.deltaTime * RotateSpeed);
        m_CharacterController.SimpleMove(m_MoveDir * MoveSpeed);
    }

    void LateUpdate()
    {
        //Move animation
        m_PlayerAnimationController.PlayMoveAnimation(m_MoveDir.magnitude, m_LastActiveMoveDir, m_TargetRot);
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
