/// <summary>
/// Отключить возможность ввода
/// </summary>
public class TriggerEvent_DisableInput : TriggerAction_Event
{
    protected override void CallEvent()
    {
        InputManager.Instance.InputIsEnabled = false;
    }
}
