using mytest.ActionTrigger.Effects;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Начать проигрывать эффект (из эффектов триггера EffectController)
    /// Реализация эффекта должна находиться на объекте
    /// Окончание эффекта отслеживаеться EffectFinishListener.
    /// </summary>
    public class TriggerEvent_PlayEffect : TriggerAction_Event
    {
        private iActionTrigger_EffectController m_EffectController;

        protected override void CallEvent()
        {
            m_EffectController.ActivateEffect_Action();
        }

        void Start()
        {
            m_EffectController = GetComponent<iActionTrigger_EffectController>();

            if (m_EffectController != null)
                m_EffectController.Init(EffectFinishedHandler);
        }

        void EffectFinishedHandler()
        {
            CallEventFinished();
        }
    }
}
