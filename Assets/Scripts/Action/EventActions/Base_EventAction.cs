using UnityEngine;

/// <summary>
/// Базовый класс для события триггера
/// </summary>
public abstract class Base_EventAction : MonoBehaviour
{
    protected iInteractable m_Trigger;

	void Start ()
    {
        m_Trigger = GetComponent<iInteractable>();
        m_Trigger.OnInteract += TriggerInteractHandler;
    }

    protected abstract void TriggerInteractHandler();
}
