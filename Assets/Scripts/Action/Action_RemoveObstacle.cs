using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_RemoveObstacle : BaseAction
{
    public Transform Target;
    public Light LightObject;
    public GameObject LockObject;

    private void FixedUpdate()
    {
        float sqrDistToTarget = (Target.position - transform.position).sqrMagnitude;
        Debug.Log(sqrDistToTarget);
        if (sqrDistToTarget <= 4)
            Prepare();
        else
            Unprepare();
    }

    void Prepare()
    {
        LightObject.color = Color.green;
        Action();
    }

    void Unprepare()
    {
        LightObject.color = Color.yellow;
    }

    public override void Action()
    {
        base.Action();

        LockObject.SetActive(false);
    }
}
