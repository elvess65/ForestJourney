using UnityEngine;

/// <summary>
/// Класс для отслеживания события взаимодейтсвия с триггером
/// </summary>
public class ActionTrigger_EventListener : MonoBehaviour
{
    public TriggerAction_Event[] OnInteractEvents;
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

/// <summary>
/// Базовый класс для компонента триггера (поведение на событие)
/// </summary>
public abstract class TriggerAction_Event : MonoBehaviour
{
    public TriggerAction_Event[] OnEventFinished;

    public abstract void StartEvent();

    protected bool CallEventFinished()
    {
        if (OnEventFinished.Length == 0)
            return false;

        for (int i = 0; i < OnEventFinished.Length; i++)
            OnEventFinished[i].StartEvent();

        return true;
    }
}
