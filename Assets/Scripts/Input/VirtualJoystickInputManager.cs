using UnityEngine;

public class VirtualJoystickInputManager : BaseInputManager
{
    public System.Action<Vector3> OnMove;
    public string MainJoystickName = "MainJoystick";

    private Vector2 m_PrevDir = Vector2.zero;

	public override void Update () 
    {
        if (!Enabled)
            return;

		Vector2 jPosition = UltimateJoystick.GetPosition(MainJoystickName);
        m_PrevDir = jPosition;

        if (OnMove != null)
            OnMove(new Vector3(jPosition.x, 0, jPosition.y));
	}
}
