using mytest.Effects.Custom;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Отключить эммиссию
    /// </summary>
    public class TriggerEvent_EmissionSetDisable : TriggerAction_Event
    {
        [Space(10)]
        public EmissionEffectBehaviour Behaviour;

        protected override void CallEvent()
        {
            Behaviour.SetDisable();
        }
    }
}
