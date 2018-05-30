using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер эффектов - Animator, который некоторое время проигрывает анимацию, а затем вызывает вторую
/// </summary>
public class ActionTrigger_EffectController_AnimatorWithDelayAction : ActionTrigger_EffectController_Animator
{
    [Header("DelayAction")]
    public string DelayAction_Key;
    public float DelayTime = 1;

    public override void ActivateEffect_Action()
    {
        base.ActivateEffect_Action();

        StartCoroutine(DelayAction());
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(DelayTime);
        AnimatorController.SetTrigger(DelayAction_Key);
    }
}
