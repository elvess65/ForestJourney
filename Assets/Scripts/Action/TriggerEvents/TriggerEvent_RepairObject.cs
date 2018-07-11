public class TriggerEvent_RepairObject : TriggerAction_Event
{
    private Object_RepairBehaviour m_Behaviour;

    private void Start()
    {
        m_Behaviour = GetComponent<Object_RepairBehaviour>();
    }

    public override void StartEvent()
    {
        m_Behaviour.Animate();
    }
}
