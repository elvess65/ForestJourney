using UnityEngine;

namespace mytest.UI.InputSystem
{
    public class VirtualJoystickInputManager : BaseInputManager
    {
        public string MainJoystickName = "MainJoystick";

        public override void UpdateInput()
        {
            Vector2 jPosition = UltimateJoystick.GetPosition(MainJoystickName);

            if (OnMove != null)
                OnMove(new Vector3(jPosition.x, 0, jPosition.y));

            if (Input.GetKeyUp(KeyCode.Space))
                OnJump();
        }
    }
}
