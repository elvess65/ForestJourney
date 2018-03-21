using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public VirtualJoystickInputManager Manager;
    public LayerMask Mask;

    Vector3 moveDir = Vector3.zero;

	void Start()
    {
        Manager.OnMove += MoveInDir;
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
        moveDir = dir;
        //Debug.Log(dir);
        GetComponent<CharacterController>().SimpleMove(dir * 3);
    }
}
