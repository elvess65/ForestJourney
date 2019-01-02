using FrameworkPackage.UI.Animations;
using mytest.UI.InputSystem;
using UnityEngine;

namespace mytest.ActionTrigger.Events
{
    /// <summary>
    /// Показать какой-то анимированный элемент UI (компас, кнопку)
    /// </summary>
    public class TriggerEvent_ShowUIAnimationController : TriggerAction_Event
    {
        [Tooltip("Проигрывать эту анимацию в дальнейшем при изменения состояния ввода")]
        public bool SubscribeShowOnInput = true;
        public UIAnimationController_Base AnimationController;

        protected override void CallEvent()
        {
            AnimationController.PlayAnimation(true);

            if (SubscribeShowOnInput)
                InputManager.Instance.OnInputStateChange += AnimationController.PlayAnimation;
        }
    }
}
