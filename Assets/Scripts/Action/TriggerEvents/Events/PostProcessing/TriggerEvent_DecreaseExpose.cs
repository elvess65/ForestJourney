using mytest.Effects.PostProcessing;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Уменьшить уровень Expose
    /// </summary>
    public class TriggerEvent_DecreaseExpose : TriggerAction_Event
    {
        protected override void CallEvent()
        {
            PostProcessingController.Instance.DecreasePostExposure();
        }
    }
}
