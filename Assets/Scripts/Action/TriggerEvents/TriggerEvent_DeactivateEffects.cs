using System.Collections;
using UnityEngine;

/// <summary>
/// Отключение эффектов
/// </summary>
public class TriggerEvent_DeactivateEffects : TriggerAction_Event
{
    public Effect_Base[] Effects;
    public float Delay = 1;

    public override void StartEvent()
    {
        if (Delay > 0)
            StartCoroutine(WaitDelay());
        else
            DeactivateEffects();
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(Delay);
        DeactivateEffects();
    }

    void DeactivateEffects()
    {
		for (int i = 0; i < Effects.Length; i++)
			Effects[i].Deactivate();
    }
}
