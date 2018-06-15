using System;
using UnityEngine;

public abstract class Actiontrigger_EffectController_Base : MonoBehaviour, iActionTrigger_EffectController
{
    public event Action OnEffectFinished;

    [Tooltip("Обаботчик окончания эффектов")]
    public ActionTrigger_EffectFinishListener EffectFinishListener;

    public void Init(Action onEffectFinished)
    {
        if (EffectFinishListener != null)
            EffectFinishListener.OnEffectFinish += onEffectFinished;
    }

    public abstract void ActivateEffect_Action();
}
