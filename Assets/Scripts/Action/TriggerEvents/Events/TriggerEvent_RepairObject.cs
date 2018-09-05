using mytest.Effects.Custom.RepairObject;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Собрать объект
    /// </summary>
    public class TriggerEvent_RepairObject : TriggerAction_Event
    {
        private RepairObjectBehaviour m_Behaviour;

        private void Start()
        {
            m_Behaviour = GetComponent<RepairObjectBehaviour>();
        }

        protected override void CallEvent()
        {
            m_Behaviour.Animate();
        }
    }
}
