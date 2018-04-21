using UnityEngine;

/// <summary>
/// Базовый класс для всех триггеров (вращение камеры, открытие преграды, конец раунда, создание врага). Активируються когда игрок наступает на них
/// </summary>
public abstract class Action_Base : MonoBehaviour, iInteractable
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

    [Header("Effects")]
    public Effect_Base[] Effects_IsActive;
    public Effect_Base[] Effects_Action;

    
    protected bool m_IsActive = true;

    public virtual void Interact()
    {
        if (!m_IsActive)
            return;

        Deactivate();

        if (Effects_Action != null)
        {
            for (int i = 0; i < Effects_Action.Length; i++)
                Effects_Action[i].Activate();
        }

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

        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Activate();
        }     
    }

    /// <summary>
    /// Перейти в неактивное состояние
    /// </summary>
    protected virtual void Deactivate()
    {
        m_IsActive = false;
        Collider.enabled = false;

        if (Effects_IsActive != null)
        {
            for (int i = 0; i < Effects_IsActive.Length; i++)
                Effects_IsActive[i].Deactivate();
        }

        if (AssistantPoint != null)
            GameManager.Instance.AssistManager.RemovePoint(AssistantPoint);
    }
}
