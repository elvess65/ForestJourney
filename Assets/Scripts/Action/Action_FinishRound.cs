public class Action_FinishRound : Action_Base
{
    public override void Action()
    {
        base.Action();

        GameManager.Instance.FinishRound();
    }
}
