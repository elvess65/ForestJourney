using System.Collections;
using UnityEngine;

/// <summary>
/// Контроллер окончания проигрывания эффектов, который вызываеться с задержкой
/// </summary>
public class ActionTrigger_EffectFinishListener_Delay : ActionTrigger_EffectFinishListener 
{
    public float Delay = 1;

	public override void OnEffectFinished()
	{
        StartCoroutine(WaiDelay());
	}

    IEnumerator WaiDelay()
    {
        yield return new WaitForSeconds(Delay);

		if (OnEffectFinish != null)
			OnEffectFinish();
    }
}
