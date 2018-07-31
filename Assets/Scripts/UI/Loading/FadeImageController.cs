using UnityEngine;
using UnityEngine.UI;

public class FadeImageController : MonoBehaviour 
{
    public static FadeImageController Instance;

    private Image m_FadeImage;
    private Utils.InterpolationData<Color> m_LerpData;

    private const float m_FADE_TIME = 2;
    private const float m_INIT_ALPHA = 1;

    void Awake()
    {
        Instance = this;

        m_FadeImage = GetComponent<Image>();

        //Начальное значение цвета
        Color color = m_FadeImage.color;
        color.a = m_INIT_ALPHA;
        m_FadeImage.color = color;
    }

	void Start () 
    {
		//Данные для анимации
		m_LerpData = new Utils.InterpolationData<Color>(m_FADE_TIME);
		m_LerpData.From = m_FadeImage.color;
		m_LerpData.To = m_LerpData.From;
		m_LerpData.To.a = 0;
        m_LerpData.Start();
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
            }
        }
	}
}
