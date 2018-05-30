using System.Collections;
using UnityEngine;

/// <summary>
/// Триггер конца раунда
/// </summary>
public class Action_FinishRound : ActionTrigger
{
    public float Delay = 2;

    public override void Interact()
    {
        base.Interact();

        StartCoroutine(FinishRoundDelay(Delay));
    }

    IEnumerator FinishRoundDelay(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.Instance.FinishRound();
    }
}
