using UnityEngine;
using UnityEngine.UI;

public abstract class UIWindow_TimerBeforeClose : UIWindow_CloseButton
{
    [Space(10)]
    public Image Image_ProgressToEnable;
    public Text Text_SecondsToEnable;

    private const float m_SECONDS_TO_ENABLE = 3;

    private float m_CurTime;
    private bool m_IsTime;

    protected override void Init()
    {
        base.Init();

        Text_SecondsToEnable.text = m_SECONDS_TO_ENABLE.ToString();
    }

    protected override void ShowAnimation_Finished()
    {
		//Перегрузить, чтобы возможность ввода не включалась автоматически

		Debug.Log("WindowShowAnimationFinished");

		m_CurTime = m_SECONDS_TO_ENABLE;
		m_IsTime = true;
    }


    void Update()
    {
        if (m_IsTime)
        {
            m_CurTime -= Time.deltaTime;
			int secondsToFinish = Mathf.CeilToInt(m_CurTime);

            Image_ProgressToEnable.fillAmount = Mathf.Lerp(0, 1, m_CurTime / m_SECONDS_TO_ENABLE);
            Text_SecondsToEnable.text = secondsToFinish.ToString();

            if (m_CurTime <= 0)
            {
                m_IsTime = false;
                Text_SecondsToEnable.text = "Tap to close";
                LockInput(false);
            }
        }
    }
}
