using UnityEngine;

public class ActionTrigger_EffectFinishListener_Animator : AbstractEffectFinishListener
{
    public override void OnEffectFinished()
    {
        if (OnEffectFinish != null)
            OnEffectFinish();
    }
}
