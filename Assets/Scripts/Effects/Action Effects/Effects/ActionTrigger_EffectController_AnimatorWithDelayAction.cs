using System.Collections;
using UnityEngine;

namespace mytest.ActionTrigger.Effects
{
    /// <summary>
    /// Контроллер эффекта анимации, который некоторое время проигрывает анимацию (Action_Key), а затем вызывает ее окончание (DelayAction_Key) (анимация активации тумблера)
    /// Время проигрывания Action_Key контролируеться DelayTime
    /// Событие окончания - завершение второй анимации.
    /// EffectFinishListener висит на AnimatorController для отслеживания окончания анимации
    /// </summary>
    public class ActionTrigger_EffectController_AnimatorWithDelayAction : ActionTrigger_EffectController_Animator
    {
        [Space(10)]
        public string DelayAction_Key;
        public float DelayTime = 1;

        public override void ActivateEffect_Action()
        {
            base.ActivateEffect_Action();

            StartCoroutine(DelayAction());
        }

        IEnumerator DelayAction()
        {
            yield return new WaitForSeconds(DelayTime);
            AnimatorController.SetTrigger(DelayAction_Key);
        }
    }
}
