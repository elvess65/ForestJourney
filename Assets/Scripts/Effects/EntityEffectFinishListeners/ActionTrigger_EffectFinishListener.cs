using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер окончания проигрывания эффектов
/// </summary>
public class ActionTrigger_EffectFinishListener : MonoBehaviour
{
    public System.Action OnEffectFinish;

    public float Delay = 0;

    public virtual void OnEffectFinished()
    {
        if (OnEffectFinish != null)
            OnEffectFinish();
    }

	IEnumerator WaitDelay()
	{
		yield return new WaitForSeconds(Delay);

		if (OnEffectFinish != null)
			OnEffectFinish();
	}
}
