using UnityEngine;

/// <summary>
/// Событие триггера - Создание лабиринта
/// </summary>
public class EventAction_ComponentBehaviour_DynamicMazeSimple : EventAction_ComponentBehaviour
{
    public bool RotateCameraOnOnteract = true;
    public GameObject DynamicMazeObject;

    public override void StartEvent()
    {
        //Включить лабиринт
        DynamicMazeObject.SetActive(true);

        //Вращать камеру
        if (RotateCameraOnOnteract)
            GameManager.Instance.CameraController.RotateRandomly();
    }
}
