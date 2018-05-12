using UnityEngine;

public class VirtualJoystickInputManager : BaseInputManager
{
    public string MainJoystickName = "MainJoystick";

	public override void UpdateInput() 
    {
        if (!Enabled)
            return;

		Vector2 jPosition = UltimateJoystick.GetPosition(MainJoystickName);

        if (OnMove != null)
            OnMove(new Vector3(jPosition.x, 0, jPosition.y));
	}
}
