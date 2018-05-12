using UnityEngine;

public class EventAction_ComponentBehaviour_DeactivateObstacle : TriggerAction_Event
{
    public Effect_Particles_StopLoop ActiveEffects;
    public BoxCollider ObstacleCollider;

    public override void StartEvent()
    {
        ObstacleCollider.enabled = false;
        ActiveEffects.Deactivate();
    }
}
