/// <summary>
/// Триггер конца раунда
/// </summary>
public class Action_FinishRound : ActionTrigger
{
    public override void Interact()
    {
        base.Interact();

        GameManager.Instance.FinishRound();
    }
}
