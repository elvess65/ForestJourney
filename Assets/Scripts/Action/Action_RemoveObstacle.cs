using UnityEngine;

public class Action_RemoveObstacle : Action_AutoAction
{
    [Header(" - DERRIVED -")]
    public GameObject LockObject;

    public override void Action()
    {
        base.Action();

        LockObject.SetActive(false);
    }
}
