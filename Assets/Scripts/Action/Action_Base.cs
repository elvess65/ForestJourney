using UnityEngine;

public abstract class Action_Base : MonoBehaviour
{
    public System.Action OnAction;

    [Header(" - BASE -")]
    [Header("Effects")]
    public Effect_Base[] Effects_IsActive;
    public Effect_Base[] Effects_Action;
    [Header("Objects")]
    public BoxCollider Collider;
    public Transform AssistantPoint;
    
    protected bool m_IsActive = true;

    public virtual void Action()
    {
        if (!m_IsActive)
            return;

        Deactivate();

        if (Effects_Action != null)
        {
            for (int i = 0; i < Effects_Action.Length; i++)
                Effects_Action[i].Activate();
        }

        if (OnAction != null)
            OnAction();
    }

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
