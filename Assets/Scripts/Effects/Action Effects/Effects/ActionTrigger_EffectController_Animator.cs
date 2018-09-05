using UnityEngine;

namespace mytest.ActionTrigger.Effects
{
    /// <summary>
    /// Контроллер эффекта анимации. Событие окончания - завершение анимации.
    /// </summary>
    public class ActionTrigger_EffectController_Animator : ActionTrigger_EffectController_Base
    {
        [Space(10)]
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
}
