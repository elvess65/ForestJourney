using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    public BoxCollider Collder;

    private bool m_IsActive = true;

    public virtual void Action()
    {
        if (!m_IsActive)
            return;

        Deactivate();
    }

    protected virtual void Acivate()
    {
        m_IsActive = true;
    }
    protected virtual void Deactivate()
    {
        m_IsActive = false;
        Collder.enabled = false;
    }

}
