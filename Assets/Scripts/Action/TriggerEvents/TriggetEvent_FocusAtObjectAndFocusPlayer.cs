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
        InputManager.Instance.InputIsEnabled = true;
        //GameManager.Instance.CameraController.FocusAt(GameManager.Instance.GameState.Player.transform);
    }
}
/* if (Input.GetKeyDown(KeyCode.O))
        {
            InputManager.Instance.InputIsEnabled = false;
            FocusSomeTimeAt(Obj.transform, 2, () =>
            {
                Debug.Log("Finished");
                InputManager.Instance.InputIsEnabled = true;
            });
        }
 */
