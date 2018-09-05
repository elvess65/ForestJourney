using System.Collections;
using UnityEngine;

namespace mytest.ActionTrigger.Effects
{
    /// <summary>
    /// Контроллер окончания проигрывания эффектов
    /// </summary>
    public class ActionTrigger_EffectFinishListener : MonoBehaviour
    {
        public System.Action OnEffectFinish;

        [Tooltip("Задержка перед вызовом события окончания эффекта")]
        public float Delay = 0;

        public virtual void OnEffectFinished()
        {
            if (Delay > 0)
                StartCoroutine(WaitDelay());
            else
            {
                if (OnEffectFinish != null)
                    OnEffectFinish();
            }
        }

        IEnumerator WaitDelay()
        {
            yield return new WaitForSeconds(Delay);

            if (OnEffectFinish != null)
                OnEffectFinish();
        }
    }
}
