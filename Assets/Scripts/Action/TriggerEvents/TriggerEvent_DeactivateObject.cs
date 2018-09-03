using UnityEngine;

/// <summary>
/// Выключить какой-то объект
/// </summary>
public class TriggerEvent_DeactivateObject : TriggerAction_Event
{

    [Space(10)]
    public GameObject DeactivateObject;

    protected override void CallEvent()
    {
        DeactivateObject.SetActive(false);
    }
}
