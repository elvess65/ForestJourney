using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator PlayerAnimator;
    [Range(0.01f, 1)]
    public float DampTime = 0.1f;

    private const string m_ANIMATION_SPEED_NAME = "speedPercent";

    public void PlayMoveAnimation(float speed)
    {
        PlayerAnimator.SetFloat(m_ANIMATION_SPEED_NAME, speed, DampTime, Time.deltaTime);
    }
}
