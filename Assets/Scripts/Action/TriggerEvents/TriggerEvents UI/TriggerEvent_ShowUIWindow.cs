﻿using System.Collections;
using UnityEngine;

public class TriggerEvent_ShowUIWindow : TriggerAction_Event 
{
	public UIWindowsTutorialLibrary.TutorialWindowTypes TutorialWindowType;
    public bool DisableInputOnStart = true;
    public bool EnableInputOnClose = true;
    public float Delay = 1;

    public override void StartEvent()
    {
        //Если нет библиотеки окон - ничего не делать
        if (UIWindowsTutorialLibrary.Instance == null)
            return;

        //Отключить ввод при использовании если нужно
        if (DisableInputOnStart)
            InputManager.Instance.InputIsEnabled = false;

        //Если нет задержки мгновенно создать окно 
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
        UIWindow_Base wnd = GameManager.Instance.UIManager.ShowWindow(UIWindowsTutorialLibrary.Instance.GetWindowPrefabByType(TutorialWindowType));
        wnd.OnWindowHided += WindowHidedHandler;
    }

    void WindowHidedHandler()
    {
		if (EnableInputOnClose)
            InputManager.Instance.InputIsEnabled = true;

        CallEventFinished();
    }
}