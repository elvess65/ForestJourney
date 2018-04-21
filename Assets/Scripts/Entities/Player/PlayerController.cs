using UnityEngine;

[RequireComponent(typeof(PlayerCollisionController))]
[RequireComponent(typeof(PlayerAnimationController))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float MoveSpeed = 3;

    private Vector3 m_MoveDir = Vector3.zero;
    private CollisionController m_CollisionController;
    private CharacterController m_CharacterController;
    private PlayerAnimationController m_PlayerAnimationController;

    private Item_Base m_Assistant;
    private WeaponController m_Weapon;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CollisionController = GetComponent<PlayerCollisionController>();
        m_PlayerAnimationController = GetComponent<PlayerAnimationController>();

        InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove += MoveInDir;
    }

    public void MoveInDir(Vector3 dir)
    {
        m_MoveDir = dir;
        
        //Move
        m_CharacterController.SimpleMove(m_MoveDir * MoveSpeed);

        if (m_MoveDir != Vector3.zero)
        {
            //Rotate in move dir
            float angle = Mathf.Atan2(m_MoveDir.x, m_MoveDir.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
        }

        //Move animation
        m_PlayerAnimationController.PlayMoveAnimation(m_MoveDir.magnitude);
    }

    public void DestroyPlayer()
    {
        InputManager.Instance.VirtualJoystickInput.OnMove -= MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove -= MoveInDir;

        m_PlayerAnimationController.PlayMoveAnimation(0);
        m_PlayerAnimationController.enabled = false;
        m_CollisionController.enabled = false;
        enabled = false;
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
        m_CollisionController.HandleCollistion(other); 
    }
}
