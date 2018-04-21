using UnityEngine;

/// <summary>
/// Базовый класс для объектов которые можно поднять и использовать (Помошник, оружие, ключи)
/// </summary>
public abstract class Item_Base : MonoBehaviour, iInteractable, iUsable
{
    public event System.Action OnUse;
    public event System.Action OnInteract;

    [Header(" - BASE -")]
	[Header("Objects")]
	[Tooltip("Коллайдер")]
    public BoxCollider Collider;

    protected bool m_Picked = false;
    protected bool m_Using = false;

    public virtual void Interact()
    {
        Collider.enabled = false;
        m_Picked = true;
    }
    public virtual void Use()
    {
        m_Picked = false;
        m_Using = true;
    }

    protected abstract void Start();
    protected abstract void Update();
}
