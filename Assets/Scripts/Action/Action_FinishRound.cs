/// <summary>
/// Триггер конца раунда
/// </summary>
public class Action_FinishRound : Action_Base
{
    public override void Interact()
    {
        base.Interact();

        GameManager.Instance.FinishRound();
    }
}
