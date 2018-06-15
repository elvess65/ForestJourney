using UnityEngine;

/// <summary>
/// Включить коллайдей преграды
/// </summary>
public class TriggerEvent_ActivteObstacle : TriggerAction_Event
{
    public Collider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = true;
    }
}
