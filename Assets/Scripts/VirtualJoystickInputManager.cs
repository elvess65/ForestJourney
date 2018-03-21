using UnityEngine;

public class VirtualJoystickInputManager : MonoBehaviour
{
    public System.Action<Vector3> OnMove;
    public string MainJoystickName = "MainJoystick";

    Vector2 prevDir = Vector2.zero;

	void Update () 
    {
		Vector2 jPosition = UltimateJoystick.GetPosition(MainJoystickName);

        //if (jPosition != prevDir)
        {
            prevDir = jPosition;

            if (OnMove != null)
                OnMove(new Vector3(jPosition.x, 0, jPosition.y));
        }
	}
}
