using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Включить коллайдер преграды
    /// </summary>
    public class TriggerEvent_ActivteObstacle : TriggerAction_Event
    {
        public Collider ObstacleCollider;

        protected override void CallEvent()
        {
            ObstacleCollider.enabled = true;
        }
    }
}
