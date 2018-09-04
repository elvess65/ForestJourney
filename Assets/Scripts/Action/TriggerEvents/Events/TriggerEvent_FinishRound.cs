namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Закончить раунд
    /// </summary>
    public class TriggerEvent_FinishRound : TriggerAction_Event
    {
        protected override void CallEvent()
        {
            GameManager.Instance.FinishRound();
        }
    }
}
