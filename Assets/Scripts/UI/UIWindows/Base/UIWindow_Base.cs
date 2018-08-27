using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Базовое окно, которое умеет показываться и прятаться
/// </summary>
public abstract class UIWindow_Base : MonoBehaviour
{
    public event System.Action OnWindowHided;
    public event System.Action OnWindowClose;

    public Text Text_Main;
    public UIAnimationController[] AnimationControllers;

	public virtual void Show()
	{
		Init();

        AnimationControllers[0].OnShowFinished += ShowAnimation_Finished;
        for (int i = 0; i < AnimationControllers.Length; i++)
            AnimationControllers[i].PlayAnimation(true);
	}

	public virtual void Hide()
	{
		if (OnWindowClose != null)
			OnWindowClose();

        AnimationControllers[0].OnHideFinished += HideAnimation_Finished;
		for (int i = 0; i < AnimationControllers.Length; i++)
            AnimationControllers[i].PlayAnimation(false);
	}

	public virtual void HideByEscape()
	{
		Hide();
	}


    protected abstract void Init();

    protected virtual void ShowAnimation_Finished()
    { }

    protected virtual void HideAnimation_Finished()
    {
		if (OnWindowHided != null)
			OnWindowHided();

		Destroy(gameObject);
    }
}
