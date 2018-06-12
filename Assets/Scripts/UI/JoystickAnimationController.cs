using System.Collections;
using UnityEngine;

public class JoystickAnimationController : MonoBehaviour 
{
    public UltimateJoystick Joystick;
	public float TimeToTargetPosition = 1.0f;
	public float StartHorizontalPosition = -50;
	public AnimationCurve CurveOnScreen;
    public AnimationCurve CurveOutOfScreen;

	private float m_TargetHorizontalPosition;
	private float m_TotalDist;

    void Start()
    {
        //Получить текущую позицию джойстика и длинну пути
		m_TargetHorizontalPosition = Joystick.customSpacing_X;
		m_TotalDist = m_TargetHorizontalPosition - StartHorizontalPosition;

        //Переместить джойстик в изначальную позицию (за экраном)
		ApplyPosition(StartHorizontalPosition);
    }

    public void PlayAnimation(bool state)
    {
        if (state)
            StartMoveOnScreen();
        else
            StartMoveOutOfScreen();
    }


	void StartMoveOnScreen()
	{
		StartCoroutine(MoveOnScreen());
	}

	IEnumerator MoveOnScreen()
	{
		float speed = 1.0f / TimeToTargetPosition;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
		{
			ApplyPosition(StartHorizontalPosition + CurveOnScreen.Evaluate(t) * m_TotalDist);
			yield return null;
		}

		ApplyPosition(m_TargetHorizontalPosition);
	}


	void StartMoveOutOfScreen()
	{
		StartCoroutine(MoveutOfScreen());
	}

	IEnumerator MoveutOfScreen()
	{
		float speed = 1.0f / TimeToTargetPosition;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
		{
			ApplyPosition(m_TargetHorizontalPosition - CurveOutOfScreen.Evaluate(t) * m_TotalDist);
			yield return null;
		}

		ApplyPosition(StartHorizontalPosition);
	}


	void ApplyPosition(float pos)
	{
		Joystick.customSpacing_X = pos;
		Joystick.UpdatePositioning();
	}
}
