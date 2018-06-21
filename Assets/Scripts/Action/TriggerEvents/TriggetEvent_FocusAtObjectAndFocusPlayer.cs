using UnityEngine;

/// <summary>
/// Фокусировка камеры на каком-то объекте, а затем возврат камеры на игрока
/// </summary>
public class TriggetEvent_FocusAtObjectAndFocusPlayer : TriggerAction_Event
{
    public float FocusingTime = 1;
    public Transform FocusingObject;

    public override void StartEvent()
    {
		InputManager.Instance.InputIsEnabled = false;
        GameManager.Instance.CameraController.FocusSomeTimeAt(FocusingObject, FocusingTime, FocusingFinishedHandler);
    }

    void FocusingFinishedHandler()
    {
        if (!CallEventFinished())
            InputManager.Instance.InputIsEnabled = true;
    }
}
