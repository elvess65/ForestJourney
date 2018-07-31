using UnityEngine;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour 
{
    public Image ProgressBar;

    private ScreenShotController m_ScreenShotController;
    private LoadingEffectsController m_LoadingEffectsController;
    private bool m_IsLoading = false;

	void Start () 
    {
        ProgressBar.gameObject.SetActive(false);

        m_ScreenShotController = GetComponent<ScreenShotController>();	
        m_LoadingEffectsController = GetComponent<LoadingEffectsController>();

        m_LoadingEffectsController.OnAnimationComplete += AnimationCompleteHandler;
	}

    void AnimationCompleteHandler()
    {
        ProgressBar.gameObject.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
    }
	
	void Update () 
    {
        if (Input.GetMouseButtonUp(0) && !m_IsLoading)
        {
            m_IsLoading = true;
			m_LoadingEffectsController.ShowBackgroundEffect(m_ScreenShotController.GetRenderTexture(),
															m_ScreenShotController.GetUIRenderTexture());
        }
	}
}
