using System.Collections.Generic;
using UnityEngine;

public class UIWindowsManager : MonoBehaviour 
{
    public RectTransform WindowParent;
    public UIWindow_Base UIWindow_ScreenFade;
    public UIWindowsLibrary WindowsLibrary;

    private UIWindow_Base m_ScreenFade;
    private Stack<UIWindow_Base> m_WindowQueue;

    void Start()
    {
        m_WindowQueue = new Stack<UIWindow_Base>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_WindowQueue.Count > 0)
                m_WindowQueue.Peek().HideByEscape();
        }
    }


    public UIWindow_Base ShowWindow(UIWindow_Base source)
    {
        if (m_WindowQueue.Count == 0)
            CreateScreenFade();

        UIWindow_Base wnd = CreateWindow(source);
        m_WindowQueue.Push(wnd);

        wnd.OnWindowClose += WindowCloseHandler;
        wnd.Show();

        return wnd;
    }

    UIWindow_Base CreateWindow(UIWindow_Base source)
    {
		UIWindow_Base wnd = Instantiate(source);
		RectTransform rTransform = wnd.GetComponent<RectTransform>();
		rTransform.SetParent(WindowParent, false);

        return wnd;
    }

    void WindowCloseHandler()
    {
        m_WindowQueue.Pop();

		if (m_WindowQueue.Count == 0)
			HideScreenFade();
    }


    void CreateScreenFade()
    {
        m_ScreenFade = CreateWindow(UIWindow_ScreenFade);
        m_ScreenFade.Show();
    }

    void HideScreenFade()
    {
        m_ScreenFade.Hide();
    }


    [System.Serializable]
    public struct UIWindowsLibrary
    {
        public UIWindow_Base UIWindow_Dummy;
    }
}
