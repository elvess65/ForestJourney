public class TriggerEvent_FinishRound : TriggerAction_Event 
{
	public override void StartEvent()
	{
		GameManager.Instance.FinishRound();
	}
}
