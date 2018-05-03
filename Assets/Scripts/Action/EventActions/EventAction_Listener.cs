using UnityEngine;

/// <summary>
/// Класс для отслеживания события взаимодейтсвия с триггером
/// </summary>
public class EventAction_Listener : MonoBehaviour
{
    public EventAction_ComponentBehaviour Behaviour;

    protected iInteractable m_Trigger;

    void Start()
    {
        m_Trigger = GetComponent<iInteractable>();
        m_Trigger.OnInteract += TriggerInteractHandler;
    }

    void TriggerInteractHandler()
    {
        Behaviour.StartEvent();
    }
}

/// <summary>
/// Базовый класс для компонента триггера (поведение на событие)
/// </summary>
public abstract class EventAction_ComponentBehaviour : MonoBehaviour
{
    public abstract void StartEvent();
}
