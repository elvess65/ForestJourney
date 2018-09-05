using mytest.ActionTrigger.Effects;
using mytest.Interaction;
using mytest.Main;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Класс триггера. Обрабатывает вход игрока в зону коллайдера. 
    /// События при взаимодейтсвии вызываються дополнительными копмпонентам ActionTrigger_EventListener
    /// </summary>
    public class ActionTrigger : MonoBehaviour, iInteractable
    {
        public event System.Action OnInteract;              //Вызываеться при взаимодействии с триггером
        public event System.Action OnInteractionFinished;   //Отслеживаеться и вызываеться если на тригере присутствует компонент iActionTrigger_EffectController

        [Header("Rotation")]
        public CameraRotation_EffectBehaviour RotationBehaviour;

        [Header("Objects")]
        [Tooltip("Точка для помошника")]
        public Transform AssistantPoint;
        [Tooltip("Массив ключей, необходимых для активации")]
        public GameStateController.KeyTypes[] ActivationKeys;

        protected bool m_IsActive = true;
        protected BoxCollider m_Collider;
        protected iActionTrigger_EffectController m_EffectController;   //Контроллер эффектов (активируеться при взаимодейтсвии, отслеживает конец еффекта и вызывает OnInteractionFinished)

        protected virtual void Start()
        {
            m_Collider = GetComponent<BoxCollider>();

            m_EffectController = GetComponent<iActionTrigger_EffectController>();
            if (m_EffectController != null)
                m_EffectController.Init(EffectFinishedHandler);
        }

        public virtual void Interact()
        {
            if (!m_IsActive)
                return;

            if (!HasEnoughKeys())
                return;

            //Отключить коллайдер, состояние и удалить из списка объектов
            Deactivate();

            //Начать проигрывать эффект
            if (m_EffectController != null)
                m_EffectController.ActivateEffect_Action();

            //Вращать камеру если нужно
            if (RotationBehaviour != null)
                RotationBehaviour.Rotate(m_EffectController);

            //Событие взаимодействия
            if (OnInteract != null)
                OnInteract();
        }


        /// <summary>
        /// Перейти в активированное состояние
        /// </summary>
        protected virtual void Acivate()
        {
            m_IsActive = true;
            m_Collider.enabled = true;

            //if (m_EffectController != null)
            //    m_EffectController.ActivateEffects_IsActive();
        }

        /// <summary>
        /// Перейти в неактивное состояние
        /// </summary>
        protected virtual void Deactivate()
        {
            m_IsActive = false;
            m_Collider.enabled = false;

            if (AssistantPoint != null)
                GameManager.Instance.AssistManager.RemovePoint(AssistantPoint);
        }

        /// <summary>
        /// Окончание проигрывания эффекта взаимодейтсвия
        /// </summary>
        protected virtual void EffectFinishedHandler()
        {
            if (OnInteractionFinished != null)
                OnInteractionFinished();
        }

        /// <summary>
        /// Проверка на достаточное количество ключей
        /// </summary>
        /// <returns></returns>
        protected bool HasEnoughKeys()
        {
            if (ActivationKeys.Length > 0 && !GameManager.Instance.GameState.HasKeysForActivation(ActivationKeys))
            {
                Debug.Log("Not enough activation keys");
                return false;
            }
            return true;
        }
    }
}
