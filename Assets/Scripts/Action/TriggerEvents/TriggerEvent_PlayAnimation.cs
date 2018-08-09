/// <summary>
/// Начать проигрывать анимацию
/// </summary>
public class TriggerEvent_PlayAnimation : TriggerAction_Event
{
    private iActionTrigger_EffectController m_EffectController;

    protected override void Event()
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
