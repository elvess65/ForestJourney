using UnityEngine;

/// <summary>
/// Контроллер эффектов - Particle System. Событие окончания - любое время.
/// </summary>
public class ActionTrigger_EffectController_Particles : Actiontrigger_EffectController_Base
{
    [Tooltip("Эффект активного объекта (Выключаеться при использовании)")]
    public Effect_Base[] Effects_IsActive;
    [Tooltip("Эффект испоьзования")]
    public Effect_Base[] Effects_Action;
    /*
    /// <summary>
    /// Активировать эффект активного объекта
    /// </summary>
    public void ActivateEffects_IsActive()
    {
        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Activate();
        }
    }

    /// <summary>
    /// Дактивировать эффект активного объекта
    /// </summary>
    public void DeactivateEffects_IsActive()
    {
        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Deactivate();
        }
    }*/

    /// <summary>
    /// Активировать эффект использования
    /// </summary>
    public override void ActivateEffect_Action()
    {
        if (Effects_Action != null)
        {
            for (int i = 0; i < Effects_Action.Length; i++)
                Effects_Action[i].Activate();
        }

		if (Effects_IsActive != null)
		{
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Deactivate();
		}

        if (EffectFinishListener != null)
            EffectFinishListener.OnEffectFinished();
    }
}
