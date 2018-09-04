using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Отключить коллайдер преграды
    /// </summary>
    public class TriggerEvent_DeactivateObstacle : TriggerAction_Event
    {
        [Space(10)]
        public Collider ObstacleCollider;

        protected override void CallEvent()
        {
            ObstacleCollider.enabled = false;
        }
    }
}
