public class TriggerEvent_ShowUIWindow : TriggerAction_Event 
{
    [UnityEngine.Space(10)]
	public UIWindowsTutorialLibrary.TutorialWindowTypes TutorialWindowType;
    public bool DisableInputOnStart = true;
    public bool EnableInputOnClose = true;

    public override void StartEvent()
    {
		//Если нет библиотеки окон - ничего не делать
		if (UIWindowsTutorialLibrary.Instance == null)
			return;

		//Отключить ввод при использовании если нужно
		if (DisableInputOnStart)
			InputManager.Instance.InputIsEnabled = false;

        base.StartEvent();
    }

    protected override void Event()
    {
        //Создать окно
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
