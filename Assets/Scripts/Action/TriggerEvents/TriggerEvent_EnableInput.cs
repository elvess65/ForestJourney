public class TriggerEvent_EnableInput : TriggerAction_Event
{
	protected override void CallEvent()
	{
		InputManager.Instance.InputIsEnabled = true;
	}
}