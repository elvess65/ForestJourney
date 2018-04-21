public class KeyController : Action_Base
{
    public GameStateController.KeyTypes Type = GameStateController.KeyTypes.Key1;

    public override void Interact()
    {
        GameManager.Instance.GameState.AddKey(Type);
        Destroy(gameObject);

        base.Interact();
    }
}
