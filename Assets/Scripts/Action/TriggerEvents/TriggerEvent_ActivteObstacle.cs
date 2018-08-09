using UnityEngine;

/// <summary>
/// Включить коллайдер преграды
/// </summary>
public class TriggerEvent_ActivteObstacle : TriggerAction_Event
{
    public Collider ObstacleCollider;

    protected override void Event()
    {
        ObstacleCollider.enabled = true;
    }
}
