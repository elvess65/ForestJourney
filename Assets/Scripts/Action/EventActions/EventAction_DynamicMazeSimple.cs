using UnityEngine;

/// <summary>
/// Событие триггера - Создание лабиринта
/// </summary>
public class EventAction_DynamicMazeSimple : Base_EventAction
{
    public bool RotateCameraOnOnteract = true;
    public GameObject DynamicMazeObject;

    protected override void TriggerInteractHandler()
    {
        //Включить лабиринт
        DynamicMazeObject.SetActive(true);

        //Вращать камеру
        if (RotateCameraOnOnteract)
            GameManager.Instance.CameraController.RotateRandomly();
    }
}
