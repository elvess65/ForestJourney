using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator PlayerAnimator;
    [Range(0.01f, 1)]
    public float DampTime = 0.1f;

    private bool m_IsPaused = false;
    private Utils.InterpolationData<float> m_LerpData;
    private const string m_ANIMATION_SPEED_NAME = "speedPercent";

    public void PlayMoveAnimation(float speed)
    {
        if (m_IsPaused)
            return;

        PlayerAnimator.SetFloat(m_ANIMATION_SPEED_NAME, speed, DampTime, Time.deltaTime);
    }

    public void StopPlayerMoveAnimation()
    {
        if (m_IsPaused)
            return;
        
        PlayerAnimator.SetFloat(m_ANIMATION_SPEED_NAME, 0);
    }

    public void PauseAnimations(bool isPaused)
    {
        m_IsPaused = isPaused;

        m_LerpData.TotalTime = 0.1f;
        if (isPaused)
        {
            m_LerpData.From = 1;
            m_LerpData.To = 0;
        }
        else 
        {
			m_LerpData.From = 0;
			m_LerpData.To = 1;
        }

        m_LerpData.Start();
    }

    private void Update()
    {
        if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment();
            PlayerAnimator.speed = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);

            if (m_LerpData.Overtime())
            {
                PlayerAnimator.speed = m_LerpData.To;
                m_LerpData.Stop();
            }
        }
    }
}
