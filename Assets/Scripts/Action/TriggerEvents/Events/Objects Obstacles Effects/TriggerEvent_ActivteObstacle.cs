using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Включить коллайдер преграды
    /// </summary>
    public class TriggerEvent_ActivteObstacle : TriggerAction_Event
    {
        [Space(10)]
        public Collider ObstacleCollider;

        void Awake()
        {
            ObstacleCollider.enabled = false;
        }

        protected override void CallEvent()
        {
            ObstacleCollider.enabled = true;
        }
    }
}
