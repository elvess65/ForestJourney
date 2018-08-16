using UnityEngine;

/// <summary>
/// Фокусировка камеры на каком-то объекте, а затем возврат на игрока
/// </summary>
public class TriggetEvent_FocusAtObjectAndFocusPlayer : TriggerAction_Event
{
    [Space(10)]
    public Transform FocusingObject;
    public float FocusingTime = 1;

    protected override void CallEvent()
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
