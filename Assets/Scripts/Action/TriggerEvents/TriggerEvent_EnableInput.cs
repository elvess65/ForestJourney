public class TriggerEvent_EnableInput : TriggerAction_Event
{
	protected override void Event()
	{
		InputManager.Instance.InputIsEnabled = true;
	}
}