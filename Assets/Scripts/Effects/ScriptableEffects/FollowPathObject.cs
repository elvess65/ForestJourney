using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathObject : FollowPathBehaviour 
{
    public string StartAnimationName = "StartAnimation";
    public string FinishAnimationName = "FinishAnimation";
    public float StartDelay = 1;
    public float DestroyDelay = 10;
    public Animator m_AnimationController;
    public ActionTrigger_EffectFinishListener AnimationFinishedListener;

    IEnumerator Start () 
    {
        yield return new WaitForSeconds(StartDelay);

        m_AnimationController.SetTrigger(StartAnimationName);
        AnimationFinishedListener.OnEffectFinish += StartAnimation_EffectFinishHandler;
	}

    protected override void Impact()
    {
        base.Impact();

        Destroy(gameObject, DestroyDelay);
    }

    void StartAnimation_EffectFinishHandler()
	{
        MoveAlongPath();
	}
}
