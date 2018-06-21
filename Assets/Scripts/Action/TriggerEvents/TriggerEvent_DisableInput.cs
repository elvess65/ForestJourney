public class TriggerEvent_DisableInput : TriggerAction_Event
{
    public override void StartEvent()
    {
        InputManager.Instance.InputIsEnabled = false;
    }
}
