using UnityEngine;

public abstract class Pickable_Base : MonoBehaviour
{
    public BoxCollider Collider;

    protected bool m_Picked = false;
    protected bool m_Using = false;

    public virtual void Pick()
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
