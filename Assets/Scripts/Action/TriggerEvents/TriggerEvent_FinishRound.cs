/// <summary>
/// Закончить раунд
/// </summary>
public class TriggerEvent_FinishRound : TriggerAction_Event 
{
	protected override void Event()
	{
		GameManager.Instance.FinishRound();
	}
}
