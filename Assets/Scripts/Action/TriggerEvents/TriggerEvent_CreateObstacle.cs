using UnityEngine;

/// <summary>
/// Создание простой преграды
/// </summary>
public class TriggerEvent_CreateObstacle : TriggerAction_Event
{
    public Collider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = true;
    }
}
