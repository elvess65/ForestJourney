using System.Collections;
using UnityEngine;

/// <summary>
/// Включить какой-то объект
/// </summary>
public class TriggerEvent_ActivateObject : TriggerAction_Event
{
    public GameObject ActivateObject;

    void Start()
    {
        ActivateObject.SetActive(false);
    }

    protected override void Event()
    {
        ActivateObject.SetActive(true);
    }
}
