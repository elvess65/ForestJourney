using UnityEngine;

/// <summary>
/// Класс для отслеживания события взаимодейтсвия с триггером
/// </summary>
public class TriggerAction_Listener : MonoBehaviour
{
    public TriggerAction_Event[] InteractEvents;

    protected iInteractable m_Trigger;

    void Start()
    {
        m_Trigger = GetComponent<iInteractable>();
        m_Trigger.OnInteract += TriggerInteractHandler;
    }

    void TriggerInteractHandler()
    {
        for (int i = 0; i < InteractEvents.Length; i++)
            InteractEvents[i].StartEvent();
    }
}

/// <summary>
/// Базовый класс для компонента триггера (поведение на событие)
/// </summary>
public abstract class TriggerAction_Event : MonoBehaviour
{
    public abstract void StartEvent();
}
