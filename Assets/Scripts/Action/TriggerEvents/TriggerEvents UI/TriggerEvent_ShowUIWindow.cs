using System.Collections;
using UnityEngine;

public class TriggerEvent_ShowUIWindow : TriggerAction_Event 
{
    public bool DisableInputOnStart = true;
    public bool EnableInputOnClose = true;
    public float Delay = 1;

    public override void StartEvent()
    {
        if (DisableInputOnStart)
            InputManager.Instance.InputIsEnabled = false;

        if (Delay > 0)
            StartCoroutine(WaitDelay());
        else
            Show();
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(Delay);
        Show();
    }

    void Show()
    {
        UIWindow_Base wnd = GameManager.Instance.UIManager.ShowWindow(GameManager.Instance.UIManager.WindowManager.WindowsLibrary.UIWindow_Dummy);
        wnd.OnWindowHided += WindowHidedHandler;
    }

    void WindowHidedHandler()
    {
		if (EnableInputOnClose)
            InputManager.Instance.InputIsEnabled = true;
    }
}
