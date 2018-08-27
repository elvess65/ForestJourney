using UnityEngine;

public class TriggerEvent_SetLayer : TriggerAction_Event
{
    [Space(10)]
    public LayerMask Layer;
    public Transform Target;

    protected override void CallEvent()
    {
        MoveToLayer(Target, Layer);
    }

	void MoveToLayer(Transform root, int layer)
	{
		root.gameObject.layer = layer;
		foreach (Transform child in root)
			MoveToLayer(child, layer);
	}
}
