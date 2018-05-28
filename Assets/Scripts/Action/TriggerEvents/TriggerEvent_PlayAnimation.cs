using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent_PlayAnimation : TriggerAction_Event
{
    public Animator AnimationController;
    public string Key;

    public override void StartEvent()
    {
        AnimationController.SetTrigger(Key);
    }
}
