using UnityEngine;

/// <summary>
/// Контроллер эффектов - Animator
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
}
