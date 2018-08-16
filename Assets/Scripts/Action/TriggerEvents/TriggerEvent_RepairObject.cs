/// <summary>
/// Собрать объект
/// </summary>
public class TriggerEvent_RepairObject : TriggerAction_Event
{
    private Object_RepairBehaviour m_Behaviour;

    private void Start()
    {
        m_Behaviour = GetComponent<Object_RepairBehaviour>();
    }

    protected override void CallEvent()
    {
        m_Behaviour.Animate();
    }
}
