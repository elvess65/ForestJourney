using UnityEngine;

/// <summary>
/// Отключенить коллайдер преграды
/// </summary>
public class TriggerEvent_DeactivateObstacle : TriggerAction_Event
{
    public Collider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = false;
    }
}
