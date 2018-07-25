using UnityEngine;

public class ScreenShotController : MonoBehaviour
{
    public Camera RenderCamera;

    public RenderTexture GetRenderTexture()
    {
		RenderTexture rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
		rt.Create();

		RenderCamera.targetTexture = rt;
		RenderCamera.Render();
		RenderCamera.targetTexture = null;

        return rt;
	}
}
