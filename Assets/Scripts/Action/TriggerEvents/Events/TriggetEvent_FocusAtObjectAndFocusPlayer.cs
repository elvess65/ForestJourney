using mytest.UI.InputSystem;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Фокусировка камеры на каком-то объекте, а затем возврат на игрока
    /// </summary>
    public class TriggetEvent_FocusAtObjectAndFocusPlayer : TriggerAction_Event
    {
        [Space(10)]
        public Transform FocusingObject;
        public float FocusingTime = 1;
        public TriggerAction_Event[] OnFocusingDelayFinishedEvents;

        protected override void CallEvent()
        {
            InputManager.Instance.InputIsEnabled = false;
            GameManager.Instance.CameraController.FocusSomeTimeAt(FocusingObject, FocusingTime, FocusingFinishedHandler, FocusingTimeFinishedHandler);
        }

        void FocusingFinishedHandler()
        {
            if (!CallEventFinished())
                InputManager.Instance.InputIsEnabled = true;
        }

        void FocusingTimeFinishedHandler()
        {
            for (int i = 0; i < OnFocusingDelayFinishedEvents.Length; i++)
                OnFocusingDelayFinishedEvents[i].StartEvent();
        }
    }
}
