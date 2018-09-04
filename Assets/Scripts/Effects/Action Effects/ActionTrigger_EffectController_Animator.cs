using System;
using UnityEngine;

/// <summary>
/// Контроллер эффекта анимации. Событие окончания - завершение анимации.
/// </summary>
public class ActionTrigger_EffectController_Animator : Actiontrigger_EffectController_Base
{
    public Animator AnimatorController;
    public string Action_Key;

    /// <summary>
    /// Активировать эффект использования
    /// </summary>
    public override void ActivateEffect_Action()
    {
        AnimatorController.SetTrigger(Action_Key);
    }

    public override void DeactivateEffect_Action()
    {
    }
}
