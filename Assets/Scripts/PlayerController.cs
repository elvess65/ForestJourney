using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float MoveSpeed = 3;
    [Range(0.01f, 1)]
    public float DampTime = 0.1f;
    [Header("References")]
    public Animator PlayerAnimator;

    private Vector3 m_MoveDir = Vector3.zero;
    private CharacterController m_CharacterController;

    private const string m_ANIMATION_SPEED_NAME = "speedPercent";
    private const string m_ACTION_FIELD_RESTART_NAME = "Restart";
    private const string m_ACTION_FIELD_ROTATE_CAMERA_NAME = "RotateCamera";
    private const string m_ACTION_FIELD_REMOVE_OBSTACLE_NAME = "RemoveObstacle";
    private const string m_ACTION_FIELD_FINISH_ROUND_NAME = "FinishRound";

    void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();

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
        PlayerAnimator.SetFloat(m_ANIMATION_SPEED_NAME, m_MoveDir.magnitude, DampTime, Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        switch(tag)
        {
            case m_ACTION_FIELD_RESTART_NAME:
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                break;
            case m_ACTION_FIELD_ROTATE_CAMERA_NAME:
                other.GetComponent<BaseAction>().Action();
                break;
            case m_ACTION_FIELD_REMOVE_OBSTACLE_NAME:
                Debug.Log("REMOVE OBSTACLE");
                break;
            case m_ACTION_FIELD_FINISH_ROUND_NAME:
                Debug.Log("FINISH ROUND");
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                break;
        }
    }
}
