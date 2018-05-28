using UnityEngine;

public abstract class AbstractEffectFinishListener : MonoBehaviour
{
    public System.Action OnEffectFinish;

    public abstract void OnEffectFinished();
}
