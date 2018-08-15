using UnityEngine;

/// <summary>
/// Отключить коллайдер преграды
/// </summary>
public class TriggerEvent_DeactivateObstacle : TriggerAction_Event
{
    [Space(10)]
    public Collider ObstacleCollider;

    protected override void Event()
    {
        ObstacleCollider.enabled = false;
    }
}
