using UnityEngine;

/// <summary>
/// Включить какой-то объект
/// </summary>
public class TriggerEvent_ActivateObject : TriggerAction_Event
{
    public GameObject ActivateObject;

    private void Start()
    {
        ActivateObject.SetActive(false);
    }

    public override void StartEvent()
    {
        ActivateObject.SetActive(true);
    }
}
