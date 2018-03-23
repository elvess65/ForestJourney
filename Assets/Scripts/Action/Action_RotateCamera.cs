using UnityEngine;

public class Action_RotateCamera : Action_Base
{
    public override void Action()
    {
        base.Action();

        GameManager.Instance.CamController.RotateRandomly();
    }
}
