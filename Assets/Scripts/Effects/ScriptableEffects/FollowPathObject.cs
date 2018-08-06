using System.Collections;
using UnityEngine;

public class FollowPathObject : FollowPathBehaviour 
{
    [Header("Animation")]
	public Animator AnimationController;
	public ActionTrigger_EffectFinishListener AnimationFinishedListener;
    public string StartAnimationName = "StartAnimation";
    public string FinishAnimationName = "FinishAnimation";
    [Header("Delays")]
    public float StartDelay = 1;
    public float DestroyDelay = 10;
    public bool MoveOnStart = true;

    IEnumerator Start () 
    {
        yield return new WaitForSeconds(StartDelay);

        if (AnimationController != null)
        {
            AnimationController.SetTrigger(StartAnimationName);
            AnimationFinishedListener.OnEffectFinish += StartAnimation_EffectFinishHandler;
        }

        if (MoveOnStart)
            MoveAlongPath();
	}

    protected override void Impact()
    {
        base.Impact();

        Destroy(gameObject, DestroyDelay);
    }

    void StartAnimation_EffectFinishHandler()
	{
        if (!MoveOnStart)
            MoveAlongPath();
	}
}
