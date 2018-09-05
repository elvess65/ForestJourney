using mytest.ActionTrigger.Events;
using mytest.Interaction;
using System.Collections;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Класс для отслеживания события взаимодейтсвия с триггером. 
    /// Содержит списки событий, которые должны вызваться
    /// Сам подписываеться на ActionTrigger.Interact/InteractionFinished
    /// </summary>
    public class ActionTrigger_EventListener : MonoBehaviour
    {
        [Tooltip("Вызываеться при взаимодействии с триггером")]
        public TriggerAction_Event[] OnInteractEvents;
        [Tooltip("Отслеживаеться и вызываеться если на тригере присутствует EffectController")]
        public TriggerAction_Event[] OnInteractionFinishedEvents;

        protected iInteractable m_Trigger;

        void Start()
        {
            m_Trigger = GetComponent<iInteractable>();
            m_Trigger.OnInteract += TriggerInteractHandler;
            m_Trigger.OnInteractionFinished += TriggerInteractionFinishedHandler;
        }

        void TriggerInteractHandler()
        {
            for (int i = 0; i < OnInteractEvents.Length; i++)
                OnInteractEvents[i].StartEvent();
        }

        void TriggerInteractionFinishedHandler()
        {
            for (int i = 0; i < OnInteractionFinishedEvents.Length; i++)
                OnInteractionFinishedEvents[i].StartEvent();
        }
    }
}

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Базовый класс для компонента триггера (поведение на событие)
    /// </summary>
    public abstract class TriggerAction_Event : MonoBehaviour
    {
        public float Delay = 0;
        [Tooltip("События окончания этого события. Вызываеться каждой реализацией и может вообще не вызываться (DisableInput, EnableInput)")]
        public TriggerAction_Event[] OnEventFinished;

        /// <summary>
        /// Вызов события с задержкой или без. Используеться для вызова события из ActionTrigger_EventListener
        /// </summary>
        public virtual void StartEvent()
        {
            if (Delay <= 0)
                CallEvent();
            else
                StartCoroutine(WaitDelay());
        }

        protected abstract void CallEvent();

        IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(Delay);

            CallEvent();
        }

        protected bool CallEventFinished()
        {
            if (OnEventFinished.Length == 0)
                return false;

            for (int i = 0; i < OnEventFinished.Length; i++)
                OnEventFinished[i].StartEvent();

            return true;
        }
    }
}
