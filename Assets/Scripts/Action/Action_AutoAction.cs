using UnityEngine;

public abstract class Action_AutoAction : Action_Base
{
    [Header(" - DERRIVED -")]
    public float DistanceToInteract = 4;

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsActive && m_IsActive)
        {
            float sqrDistToTarget = (GameManager.Instance.GameState.Player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistToTarget <= DistanceToInteract)
                Interact();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(DistanceToInteract));
    }
}
