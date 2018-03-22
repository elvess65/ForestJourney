using UnityEngine;

public class Action_RotateCamera : BaseAction
{
    public GameObject[] ObjectsToDeactivate;
    public Light LightObject;

    public override void Action()
    {
        base.Action();

        CameraController.Instance.RotateRandomly();
    }

    protected override void Acivate()
    {
        base.Acivate();
    }

    protected override void Deactivate()
    {
        base.Deactivate();

        for (int i = 0; i < ObjectsToDeactivate.Length; i++)
            ObjectsToDeactivate[i].SetActive(false);

        LightObject.color = Color.green;
    }
}
