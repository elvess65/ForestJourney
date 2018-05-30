using UnityEngine;

/// <summary>
/// Контроллер окончания проигрывания эффектов
/// </summary>
public class ActionTrigger_EffectFinishListener : MonoBehaviour
{
    public System.Action OnEffectFinish;

    public virtual void OnEffectFinished()
    {
        if (OnEffectFinish != null)
            OnEffectFinish();
    }
}
