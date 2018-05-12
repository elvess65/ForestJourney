using UnityEngine;

/// <summary>
/// Класс триггера. Осуществляет проверку на наличие ключей и показывает эффекты.
/// Дополнительные действия осуществляються дополнительными копмпонентами на событие OnIntercat()
/// </summary>
public class ActionTrigger : MonoBehaviour, iInteractable
{
    public event System.Action OnInteract;

    [Header(" - BASE -")]
    [Tooltip("Вращать ли камеру при взаимодействии")]
    public bool RotateCameraOnInteract = false;

    [Header("Objects")]
    [Tooltip("Коллайдер")]
    public BoxCollider Collider;
    [Tooltip("Точка для помошника")]
    public Transform AssistantPoint;
    [Tooltip("Массив ключей, необходимых для активации")]
    public GameStateController.KeyTypes[] ActivationKeys;

    protected bool m_IsActive = true;
    protected ActionTrigger_Effects_Base m_EffectController;

    void Start()
    {
        m_EffectController = GetComponent<ActionTrigger_Effects_Base>();
    }

    public virtual void Interact()
    {
        if (!m_IsActive)
            return;

        if (!HasEnoughKeys())
            return;

        Deactivate();

        if (m_EffectController != null)
            m_EffectController.ActivateEffects_Action();

        if (RotateCameraOnInteract)
            GameManager.Instance.CameraController.RotateRandomly();

        if (OnInteract != null)
            OnInteract();
    }

    /// <summary>
    /// Перейти в активированное состояние
    /// </summary>
    protected virtual void Acivate()
    {
        m_IsActive = true;
        Collider.enabled = true;

        if (m_EffectController != null)
            m_EffectController.ActivateEffects_IsActive();
    }

    /// <summary>
    /// Перейти в неактивное состояние
    /// </summary>
    protected virtual void Deactivate()
    {
        m_IsActive = false;
        Collider.enabled = false;

        if (m_EffectController != null)
            m_EffectController.DeactivateEffects_IsActive();

        if (AssistantPoint != null)
            GameManager.Instance.AssistManager.RemovePoint(AssistantPoint);
    }

    /// <summary>
    /// Проверка на достаточное количество ключей
    /// </summary>
    /// <returns></returns>
    bool HasEnoughKeys()
    {
        if (ActivationKeys.Length > 0 && !GameManager.Instance.GameState.HasKeysForActivation(ActivationKeys))
        {
            Debug.Log("Not enough activation keys");
            return false;
        }
        return true;
    }
}
