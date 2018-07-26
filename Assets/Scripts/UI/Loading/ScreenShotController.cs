using UnityEngine;

public class ScreenShotController : MonoBehaviour
{
    public Camera RenderCamera;
    public Camera UIRenderCamera;

    public RenderTexture GetRenderTexture()
    {
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
		rt.Create();

		RenderCamera.targetTexture = rt;
		RenderCamera.Render();
		RenderCamera.targetTexture = null;

        return rt;
	}

	public RenderTexture GetUIRenderTexture()
	{
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
		rt.Create();

		UIRenderCamera.targetTexture = rt;
		UIRenderCamera.Render();
		UIRenderCamera.targetTexture = null;

		return rt;
	}
}
