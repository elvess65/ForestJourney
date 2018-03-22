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

    bool grounded = false;
    /*private void Update()
    {
        Debug.DrawRay(transform.position + new Vector3(0.5f, 0, 0.5f), -transform.up, Color.green);
        Debug.DrawRay(transform.position + new Vector3(-0.5f, 0, 0.5f), -transform.up, Color.green);
        grounded = false;

        RaycastHit hit;//For Detect Sureface/Base.
        if (Physics.Raycast(transform.position + new Vector3(0, 0, 0.5f), -transform.up, out hit, 1, Mask))
        {
            grounded = true;

            if (moveDir != Vector3.zero)
            {
                Vector3 surfaceNormal = hit.normal; 
                Debug.DrawRay(transform.position, surfaceNormal * 10, Color.red);

                float dot = Vector3.Dot(moveDir, surfaceNormal);
                Vector3 vc = moveDir - dot * surfaceNormal;
                moveDir = vc;
                Debug.DrawRay(transform.position, moveDir * 10, Color.yellow);

                //Vector3 forwardRelativeToSurfaceNormal = Vector3.Cross(transform.right, surfaceNormal);
                //moveDir.y = forwardRelativeToSurfaceNormal.y;h
                //lookDir.y = moveDir.y;
            }
            //Quaternion targetRotation = Quaternion.LookRotation(forwardRelativeToSurfaceNormal, surfaceNormal); //check For target Rotation.
            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 2);
        }

        if (!grounded)
            moveDir = new Vector3(0, -20 * Time.deltaTime, 0);

        transform.Translate(moveDir * Time.deltaTime * 10, Space.World);
    }*/

    public void MoveInDir(Vector3 dir)
    {
        m_MoveDir = dir;
        
        //Move
        m_CharacterController.SimpleMove(m_MoveDir * MoveSpeed);

        //Rotate in move dir
        float angle = Mathf.Atan2(m_MoveDir.x, m_MoveDir.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

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
