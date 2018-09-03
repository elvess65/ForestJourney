using UnityEngine;

/// <summary>
/// Класс триггера. Осуществляет проверку на наличие ключей и показывает эффекты.
/// Дополнительные действия осуществляються дополнительными копмпонентами на события OnIntercat() 
/// и OnInteractionFinished()
/// </summary>
public class ActionTrigger : MonoBehaviour, iInteractable
{
    public event System.Action OnInteract;
    public event System.Action OnInteractionFinished;

    [Header("Rotation")]
    [Tooltip("Вращать ли камеру при взаимодействии")]
    public bool RotateCameraOnInteract = false;
	public float Angle = 45;
	public float Speed = -1;
	public bool Clockwise = true;
    public GameObject RotateCameraEffectGrahics;
	public TriggerAction_Event[] OnRotationFinished;
    public CompassWorldBehaviour C;

    [Header("Objects")]
    [Tooltip("Точка для помошника")]
    public Transform AssistantPoint;
    [Tooltip("Массив ключей, необходимых для активации")]
    public GameStateController.KeyTypes[] ActivationKeys;

    protected bool m_IsActive = true;
    protected BoxCollider m_Collider;
    protected iActionTrigger_EffectController m_EffectController;
    
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
        if (RotateCameraOnInteract)
        {
            Vector3 offset = GameManager.Instance.GameState.Player.MoveDir * GameManager.Instance.GameState.Player.MoveSpeed * (PlayerController.ReduceSpeedAtLockInputTime / 1.5f);
            Vector3 targetPos = GameManager.Instance.GameState.Player.transform.position + offset;
            targetPos.y = RotateCameraEffectGrahics.transform.position.y;
            RotateCameraEffectGrahics.transform.position = targetPos;
            
            GameManager.Instance.CameraController.OnRotationFinished += RotationFinishedHandler;
            GameManager.Instance.CameraController.RotateAroundTarget(Angle, Speed, Clockwise);

            GameManager.Instance.GameState.CompassWorld.Show();
        }

        //Событие взаимодействия
        if (OnInteract != null)
            OnInteract();
    }

    void RotationFinishedHandler()
    {
        GameManager.Instance.CameraController.OnRotationFinished -= RotationFinishedHandler;

        GameManager.Instance.GameState.CompassWorld.Animate();

        //Начать проигрывать эффект
        if (m_EffectController != null)
            m_EffectController.DeactivateEffect_Action();

        for (int i = 0; i < OnRotationFinished.Length; i++)
            OnRotationFinished[i].StartEvent();
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
