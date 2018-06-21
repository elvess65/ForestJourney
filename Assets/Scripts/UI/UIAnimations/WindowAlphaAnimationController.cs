using UnityEngine;

public class WindowAlphaAnimationController : UIAnimationController 
{
	[Header("Link")]
    public CanvasGroup Group;

	protected override void SetTargetPosition()
	{
        m_TargetPosition = Group.alpha;
	}

	protected override void ApplyPosition(float position)
	{
        Group.alpha = position;
	}
}
