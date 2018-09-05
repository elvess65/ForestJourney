using mytest.Effects;
using UnityEngine;

namespace mytest.ActionTrigger.Effects
{
    /// <summary>
    /// Контроллер эффектов - Particle System. Событие окончания - любое время, потому что нельзя точно сказать когда выключились частицы
    /// </summary>
    public class ActionTrigger_EffectController_Particles : ActionTrigger_EffectController_Base
    {
        [Tooltip("Эффект активного объекта (Выключаеться при использовании)")]
        public Effect_Base[] Effects_IsActive;
        [Tooltip("Эффект испоьзования")]
        public Effect_Base[] Effects_Action;

        /// <summary>
        /// Активировать эффект использования
        /// </summary>
        public override void ActivateEffect_Action()
        {
            //Влючить эффект использования
            if (Effects_Action != null)
            {
                for (int i = 0; i < Effects_Action.Length; i++)
                    Effects_Action[i].Activate();
            }

            //Выключить эффект активного объекта
            if (Effects_IsActive != null)
            {
                for (int i = 0; i < Effects_IsActive.Length; i++)
                    Effects_IsActive[i].Deactivate();
            }

            if (EffectFinishListener != null)
                EffectFinishListener.OnEffectFinished();
        }

        public override void DeactivateEffect_Action()
        {
            //Выключить эффект использования
            if (Effects_Action != null)
            {
                for (int i = 0; i < Effects_Action.Length; i++)
                    Effects_Action[i].Deactivate();
            }
        }
    }
}
