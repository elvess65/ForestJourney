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

    private AssistantController m_Assistant;
    private Action_Soul m_Soul;

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CollisionController = GetComponent<PlayerCollisionController>();
        m_PlayerAnimationController = GetComponent<PlayerAnimationController>();

        InputManager.Instance.VirtualJoystickInput.OnMove += MoveInDir;
        InputManager.Instance.KeyboardInput.OnMove += MoveInDir;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
            UseAssistant();

        if (Input.GetKeyDown(KeyCode.I))
            UseSoul();
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


    public void AddAssistant(AssistantController assistant)
    {
        m_Assistant = assistant;
    }

    public void AddSoul(Action_Soul soul)
    {
        m_Soul = soul;
    }

    void UseAssistant()
    {
        if (m_Assistant != null)
        {
            m_Assistant.Assist();
            m_Assistant = null;
        }
    }

    void UseSoul()
    {
        if (m_Soul != null)
        {
            m_Soul.Activate();
            m_Soul = null;

            EnemyController enemy = FindObjectOfType<EnemyController>();
            GameObject ob = Instantiate(enemy.ExplisionPrefab);
            ob.transform.position = enemy.transform.position;
            Destroy(ob, 3);
            Destroy(enemy.gameObject);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        m_CollisionController.HandleCollistion(other); 
    }
}
