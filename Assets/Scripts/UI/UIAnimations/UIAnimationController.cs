using System.Collections;
using UnityEngine;

public abstract class UIAnimationController : MonoBehaviour 
{
    public event System.Action OnShowFinished;
    public event System.Action OnHideFinished;

    [Header("Animation")]
	public float TimeToTargetPosition = 1.0f;
	public float StartPosition = -50;
	public AnimationCurve CurveShow;
	public AnimationCurve CurveHide;

    protected float m_TargetPosition = 0;
    private float m_TotalDistance = 0;

    protected virtual void Awake()
    {
        SetTargetPosition();

		m_TotalDistance = m_TargetPosition - StartPosition;

		ApplyPosition(StartPosition);
    }

    public void PlayAnimation(bool playShowAnimation)
	{
		if (playShowAnimation)
            StartCoroutine(Show());
		else
            StartCoroutine(Hide());
	}

    IEnumerator Show()
	{
        ShowStarted();
		float speed = 1.0f / TimeToTargetPosition;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
		{
			ApplyPosition(GetShowValue(t));
			yield return null;
		}

		ApplyPosition(m_TargetPosition);
        ShowFinished();
	}

	float GetShowValue(float t)
	{
		return StartPosition + CurveShow.Evaluate(t) * m_TotalDistance;
	}

    protected virtual void ShowStarted()
    {
    }

    protected virtual void ShowFinished()
    {
        if (OnShowFinished != null)
            OnShowFinished();
    }


	IEnumerator Hide()
	{
        HideStarted();
		float speed = 1.0f / TimeToTargetPosition;

		for (float t = 0.0f; t < 1.0f; t += Time.deltaTime * speed)
		{
			ApplyPosition(GetHideValue(t));
			yield return null;
		}

		ApplyPosition(StartPosition);
        HideFinished();
	}

    float GetHideValue(float t)
    {
        return m_TargetPosition - CurveHide.Evaluate(t) * m_TotalDistance;
    }

	protected virtual void HideStarted()
	{
	}

	protected virtual void HideFinished()
	{
        if (OnHideFinished != null)
            OnHideFinished();
	}


	[ContextMenu("Show")]
	public void ContextMenuShow()
	{
		StartCoroutine(Show());
	}

	[ContextMenu("Hide")]
	public void ContextMenuHide()
	{
		StartCoroutine(Hide());
	}


    protected abstract void SetTargetPosition();

    protected abstract void ApplyPosition(float position);
}
