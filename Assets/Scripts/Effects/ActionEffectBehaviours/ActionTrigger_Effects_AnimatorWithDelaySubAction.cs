using System.Collections;
using UnityEngine;

public class ActionTrigger_Effects_AnimatorWithDelaySubAction : ActionTrigger_Effects_Animator
{
    public string Subaction_Key;
    public float Delay = 1;

    public override void ActivateEffects_Action()
    {
        base.ActivateEffects_Action();

        StartCoroutine(DelayAction());
    }

    IEnumerator DelayAction()
    {
        yield return new WaitForSeconds(Delay);
        AnimatorController.SetTrigger(Subaction_Key);
    }
}
