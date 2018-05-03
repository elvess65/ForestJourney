using UnityEngine;

public class EventAction_ComponentBehaviour_DynamicObstacle : EventAction_ComponentBehaviour
{
    public Effect_Particles_StopLoop ActiveEffects;
    public BoxCollider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = false;
        ActiveEffects.Deactivate();
    }
}
