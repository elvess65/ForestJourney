using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour 
{
    private ScreenShotController m_ScreenShotController;
    private LoadingEffectsController m_LoadingEffectsController;

	void Start () 
    {
        m_ScreenShotController = GetComponent<ScreenShotController>();	
        m_LoadingEffectsController = GetComponent<LoadingEffectsController>();
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.W))
            m_LoadingEffectsController.ShowBackgroundEffect(m_ScreenShotController.GetRenderTexture(),
                                                            m_ScreenShotController.GetUIRenderTexture());
	}
}
