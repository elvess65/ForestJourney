using UnityEngine;

public abstract class UIWindow_Base : MonoBehaviour
{
    public event System.Action OnWindowShowed;
    public event System.Action OnWindowHided;

    void Init()
    {
        
    }

    public void Show()
    {
        //TODO Wait animation
		if (OnWindowShowed != null)
			OnWindowShowed();
    }

    public void Hide()
    {
		//TODO Wait animation
		if (OnWindowHided != null)
            OnWindowHided();
    }
}
