using mytest.Effects.Custom.RepairObject;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Проиграть анимацию объекту, который может быть разобраным или сораным (анимация зависит от начального состояния)
    /// </summary>
    public class TriggerEvent_AnimateRepairObject : TriggerAction_Event
    {
        [Space(10)]
        public RepairObjectBehaviour Behaviour;

        private void Start()
        {
            if (Behaviour == null)
                Behaviour = GetComponent<RepairObjectBehaviour>();
        }

        protected override void CallEvent()
        {
            Behaviour.Animate();
        }
    }
}
