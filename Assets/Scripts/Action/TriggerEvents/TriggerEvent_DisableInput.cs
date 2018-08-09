/// <summary>
/// Отключить возможность ввода
/// </summary>
public class TriggerEvent_DisableInput : TriggerAction_Event
{
    protected override void Event()
    {
        InputManager.Instance.InputIsEnabled = false;
    }
}
