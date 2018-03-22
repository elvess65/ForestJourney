using UnityEngine;

public abstract class BaseAction : MonoBehaviour
{
    [Header(" - BASE -")]
    [Header("Effects")]
    public GameObject ActiveEffect;
    [Header("Objects")]
    public BoxCollider Collder;
    
    protected bool m_IsActive = true;

    public virtual void Action()
    {
        if (!m_IsActive)
            return;

        Deactivate();
    }

    protected virtual void Acivate()
    {
        m_IsActive = true;

        Collder.enabled = true;
        ActiveEffect.SetActive(true);
    }
    protected virtual void Deactivate()
    {
        m_IsActive = false;

        Collder.enabled = false;
        ActiveEffect.SetActive(false);
    }
}
