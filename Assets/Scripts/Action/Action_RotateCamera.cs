using UnityEngine;

public class Action_RotateCamera : BaseAction
{
    public override void Action()
    {
        base.Action();

        GameManager.Instance.CamController.RotateRandomly();
    }
}
