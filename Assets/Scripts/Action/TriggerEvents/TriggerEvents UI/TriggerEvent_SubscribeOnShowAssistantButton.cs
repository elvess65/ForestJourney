public class TriggerEvent_SubscribeOnShowAssistantButton : TriggerAction_Event 
{
    public override void StartEvent()
    {
        InputManager.Instance.OnInputStateChange += GameManager.Instance.UIManager.AssistantButtonAnimationController.PlayAnimation;
        InputManager.Instance.InputIsEnabled = true;
    }
}
