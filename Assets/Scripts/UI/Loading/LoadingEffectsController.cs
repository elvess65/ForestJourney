using UnityEngine;
using UnityEngine.UI;

public class LoadingEffectsController : MonoBehaviour
{
    public RawImage BackgroundImage;
    public Text TitleText;

	private Color m_InitColor;
	private Color m_TargetColor;
	private bool m_IsLerpingColor;
	private float m_CurTime = 0;
	private float m_TotalTime = 2;

    public void ShowBackgroundEffect(RenderTexture texture)
    {
		BackgroundImage.texture = texture;

		m_InitColor = BackgroundImage.color;
        m_TargetColor = m_InitColor;
        m_TargetColor.a = 1;

		m_CurTime = 0;
		m_IsLerpingColor = true;
    }
	
	void Update () 
    {
		if (m_IsLerpingColor)
		{
			m_CurTime += Time.deltaTime;
            Color textColor = Color.white;
            textColor.a = Mathf.Lerp(0, 1, m_CurTime / m_TotalTime);
            TitleText.color = textColor;
			BackgroundImage.color = Color.Lerp(m_InitColor, m_TargetColor, m_CurTime / m_TotalTime);

			if (m_CurTime >= m_TotalTime)
				m_IsLerpingColor = false;
		}
	}
}
