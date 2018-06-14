public class JoystickAnimationController : UIAnimationController 
{
    public UltimateJoystick Joystick;

    protected override void SetTargetPosition()
    {
		//Получить текущую позицию джойстика
		m_TargetPosition = Joystick.customSpacing_X;
    }

	protected override void ApplyPosition(float position)
	{
		Joystick.customSpacing_X = position;
		Joystick.UpdatePositioning();
	}
}
