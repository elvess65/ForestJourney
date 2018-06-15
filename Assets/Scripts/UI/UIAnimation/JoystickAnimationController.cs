using UnityEngine;

public class JoystickAnimationController : UIAnimationController 
{
    public UltimateJoystick Joystick;
    public RectTransform ThumbRectTransform;

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

	protected override void ShowStarted()
    {
        base.ShowStarted();

		ThumbRectTransform.anchoredPosition = Vector2.zero;
		Joystick.enabled = false;
	}

    protected override void ShowFinished()
    {
        base.ShowFinished();

		ThumbRectTransform.anchoredPosition = Vector2.zero;
		Joystick.enabled = true;
    }

    protected override void HideStarted()
    {
		base.HideStarted();

        ThumbRectTransform.anchoredPosition = Vector2.zero;
        Joystick.enabled = false;
    }

    protected override void HideFinished()
    {
        base.HideFinished();

        ThumbRectTransform.anchoredPosition = Vector2.zero;
        Joystick.enabled = true;
    }
}
