using System;
using UnityEngine;

namespace mytest.ActionTrigger.Effects
{
    /// <summary>
    /// Базовый класс для эффектов триггера (левитирующий объект, запуск снаряда). Окончание эффекта отслеживаеться EffectFinishListener, который вызывает соответствующее событие
    /// </summary>
    public abstract class ActionTrigger_EffectController_Base : MonoBehaviour, iActionTrigger_EffectController
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

        public abstract void DeactivateEffect_Action();
    }

    /// <summary>
    /// Интерфейс контроллера эффектов для триггеров.
    /// Содерджит функции включения/выключения эффекта взаимодейтсвия
    /// Может присутствовать на триггере или нет. Если присутсвует - отслеживает событие OnInteractionFinished
    /// </summary>
    public interface iActionTrigger_EffectController
    {
        event Action OnEffectFinished;

        void Init(Action onEffectFinished);

        /// <summary>
        /// Активировать эффект использования
        /// </summary>
        void ActivateEffect_Action();

        /// <summary>
        /// Деактивировать эффект использования 
        /// </summary>
        void DeactivateEffect_Action();
    }
}
