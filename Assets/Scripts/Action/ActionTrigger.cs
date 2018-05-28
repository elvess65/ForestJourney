using UnityEngine;

/// <summary>
/// Класс триггера. Осуществляет проверку на наличие ключей и показывает эффекты.
/// Дополнительные действия осуществляються дополнительными копмпонентами на событие OnIntercat()
/// </summary>
public class ActionTrigger : MonoBehaviour, iInteractable
{
    public event System.Action OnInteract;
    public event System.Action OnInteractionFinished;

    [Header(" - BASE -")]
    [Tooltip("Вращать ли камеру при взаимодействии")]
    public bool RotateCameraOnInteract = false;

    [Header("Objects")]
    [Tooltip("Обаботчик окончания эффектов")]
    public AbstractEffectFinishListener EffectFinishListener;
    [Tooltip("Точка для помошника")]
    public Transform AssistantPoint;
    [Tooltip("Массив ключей, необходимых для активации")]
    public GameStateController.KeyTypes[] ActivationKeys;

    protected bool m_IsActive = true;
    protected BoxCollider m_Collider;
    protected iActionTriggerEffect m_EffectController;
    
    void Start()
    {
        m_Collider = GetComponent<BoxCollider>();
        m_EffectController = GetComponent<iActionTriggerEffect>();

        if (EffectFinishListener != null)
            EffectFinishListener.OnEffectFinish += OnEffectFinished;
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
        m_Collider.enabled = true;

        if (m_EffectController != null)
            m_EffectController.ActivateEffects_IsActive();
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
    protected virtual void OnEffectFinished()
    {
        if (OnInteractionFinished != null)
            OnInteractionFinished();
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
