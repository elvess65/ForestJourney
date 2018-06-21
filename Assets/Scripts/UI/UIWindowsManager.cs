using UnityEngine;

public class UIWindowsManager : MonoBehaviour 
{
    public UIWindow_Base DummyWindow;
    public RectTransform WindowParent;

    public void ShowWindow(UIWindow_Base source)
    {
        UIWindow_Base wnd = Instantiate(source);
        wnd.transform.parent = WindowParent;
        wnd.Show();
    }
}
