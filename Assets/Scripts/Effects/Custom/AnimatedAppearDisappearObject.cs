using mytest.ActionTrigger.Effects;
using UnityEngine;

/// <summary>
/// Объект, который появляеться по анимации и пропадает с анимацией (Круг при выделении объекта)
/// </summary>
public class AnimatedAppearDisappearObject : MonoBehaviour
{
    public System.Action OnAnimationFinished;

    private Animator m_Animator;
    private ActionTrigger_EffectFinishListener m_AnimationFinishedListener;
    private string AppearAnimationName = "Appear";
    private string DisappearAnimationName = "Disappear";

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_AnimationFinishedListener = GetComponent<ActionTrigger_EffectFinishListener>();
        m_AnimationFinishedListener.OnEffectFinish += AnimationFinishedHandler;

        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        m_Animator.SetTrigger(AppearAnimationName);
    }

    public void Hide()
    {
        m_Animator.SetTrigger(DisappearAnimationName);
    }

    void AnimationFinishedHandler()
    {
        if (OnAnimationFinished != null)
        {
            OnAnimationFinished();
            OnAnimationFinished = null;
        }
    }
}