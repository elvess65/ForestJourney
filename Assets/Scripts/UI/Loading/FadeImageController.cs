using UnityEngine;
using UnityEngine.UI;

public class FadeImageController : MonoBehaviour 
{
    public System.Action OnFadeAnimationComplete;

    private Image m_FadeImage;
    private Utils.InterpolationData<Color> m_LerpData;

    private const float m_FADE_TIME = 2;
    private const float m_INIT_ALPHA = 1;

    void Awake()
    {
        m_FadeImage = GetComponent<Image>();

        //Начальное значение цвета
        Color color = m_FadeImage.color;
        color.a = m_INIT_ALPHA;
        m_FadeImage.color = color;
    }

	void Update () 
    {
        if (m_LerpData.IsStarted)	
        {
            m_LerpData.Increment(Time.deltaTime);
            m_FadeImage.color = Color.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
            if (m_LerpData.Overtime())
            {
                m_LerpData.Stop();
                m_FadeImage.color = m_LerpData.To;

                if (OnFadeAnimationComplete != null)
                {
                    OnFadeAnimationComplete();
                    OnFadeAnimationComplete = null;
                }
            }
        }
	}


	public void FadeIn()
	{
        OnFadeAnimationComplete += () => m_FadeImage.enabled = false;
        Fade(0);
	}

	public void FadeOut()
	{
        m_FadeImage.enabled = true;
        Fade(1);
	}

    void Fade(float alpha)
    {
        m_FadeImage.enabled = true;
		m_LerpData = new Utils.InterpolationData<Color>(m_FADE_TIME);
		m_LerpData.From = m_FadeImage.color;
		m_LerpData.To = m_LerpData.From;
		m_LerpData.To.a = alpha;
		m_LerpData.Start();
    }
}
