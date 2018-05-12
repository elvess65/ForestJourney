using UnityEngine;

/// <summary>
/// Отключение простой преграды
/// </summary>
public class TriggerEvent_RemoveObstacle : TriggerAction_Event
{
    public Collider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = false;
    }
}
