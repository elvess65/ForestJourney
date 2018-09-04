using mytest.UI.InputSystem;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Включить возможность ввода
    /// </summary>
    public class TriggerEvent_EnableInput : TriggerAction_Event
    {
        protected override void CallEvent()
        {
            InputManager.Instance.InputIsEnabled = true;
        }
    }
}