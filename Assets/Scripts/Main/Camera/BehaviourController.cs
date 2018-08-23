using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CameraAligningBehaviour))]
[RequireComponent(typeof(CameraRotationBehaviour))]
[RequireComponent(typeof(CameraFollowingBehaviour))]
[RequireComponent(typeof(CameraFocusingBehaviour))]
public class BehaviourController : MonoBehaviour 
{
    public Vector3 InitCameraOffset = new Vector3(0, 12, -10);
 
    private CameraAligningBehaviour m_AligningBehaviour;
    private CameraRotationBehaviour m_RotationBehaviour;
    private CameraFollowingBehaviour m_FollowingBehaviour;
    private CameraFocusingBehaviour m_FocusingBehaviour;

    private Transform m_Target;
    private Vector3 m_CachedOffset;

    //Focusing some time 
	private float m_FocusingTime;
	private System.Action m_OnFocusingFinished;
	private System.Action m_OnFocusDelayFinished;

    void Start()
    {
        m_AligningBehaviour = GetComponent<CameraAligningBehaviour>();
        m_RotationBehaviour = GetComponent<CameraRotationBehaviour>();
        m_FollowingBehaviour = GetComponent<CameraFollowingBehaviour>();
        m_FocusingBehaviour = GetComponent<CameraFocusingBehaviour>();
    }

    public void Init(Transform target)
    {
        m_Target = target;

        AlignToInitOffset();
    }

	/// <summary>
	/// Выровнять камеру согласно начальному отступу
	/// </summary>
	void AlignToInitOffset()
	{
		m_AligningBehaviour.OnFinished += FollowTarget;
		m_AligningBehaviour.AlignToOffset(m_Target, InitCameraOffset);
	}

	/// <summary>
	/// Начать следовать за целью, закешировав отступ
	/// </summary>
	void FollowTarget()
    {
        m_FollowingBehaviour.Follow(m_Target, CacheOffset());
    }


    /// <summary>
    /// Переместить камеру на некоторое время на объект, а затем вернуть на предыдущую цель
    /// </summary>
    /// <param name="target">Объект для фокусировки</param>
    /// <param name="focusingTime">Время фокусировки на объекте</param>
    /// <param name="onFocusingFinished">Окончания фокусировки (камера вернулась на предыдущую цель)</param>
    /// <param name="onFocusDelayFinished">Окончание задержки фокусировки (камера начинает возвращаться на предыдущую цель)</param>
    public void FocusSomeTimeAt(Transform target, float focusingTime, System.Action onFocusingFinished, System.Action onFocusDelayFinished)
    {
        m_FollowingBehaviour.PauseFollowing();

        m_FocusingTime = focusingTime;
        m_OnFocusingFinished = onFocusingFinished;
        m_OnFocusDelayFinished = onFocusDelayFinished;

        m_FocusingBehaviour.OnFinished += FocusedOnTargetHandler;
        m_FocusingBehaviour.MoveToWithOffset(target, CacheOffset());   //Кеш оффсета на момент начала движения (если было вращение камеры)
    }

    void FocusedOnTargetHandler()
    {
        StartCoroutine(WaitFocusDelay());
    }

    IEnumerator WaitFocusDelay()
    {
        yield return new WaitForSeconds(m_FocusingTime);

        if (m_OnFocusDelayFinished != null)
            m_OnFocusDelayFinished();

		m_FocusingBehaviour.OnFinished += FocusingFinishedHandler;
        m_FocusingBehaviour.MoveToWithOffset(m_Target, m_CachedOffset);    
    }

    void FocusingFinishedHandler()
    {
		if (m_OnFocusingFinished != null)
			m_OnFocusingFinished();

        m_FollowingBehaviour.ContinueFollowing();
    }

	//bool lockInput = false, bool clockWise = true, float angleSpeed = 405
    //if (lockInput)
    //    InputManager.Instance.InputIsEnabled = false;
    //    OnCameraArrived += InputManager.Instance.UnlockInput;
	public void RotateAroundTarget()
    {
        //TODO: Pause animation

        //Если нужно вращать камеру, а она все еще следует за персонажем подписаться на событие окончания движения
        if (m_FollowingBehaviour.IsMoving)
            m_FollowingBehaviour.OnFinished += RotateAroundTarget;
        else 
        {
            m_FollowingBehaviour.PauseFollowing();
            m_RotationBehaviour.OnFinished += RotationFinishedHandler;
            m_RotationBehaviour.RotateAroundBy(m_Target, 90, true);
        }
    }

	void RotationFinishedHandler()
	{
        m_FollowingBehaviour.Follow(m_Target, CacheOffset());
	}


	void LateUpdate () 
    {
		if (!GameManager.Instance.IsActive && m_Target == null)
			return;

        m_AligningBehaviour.UpdateBehaviour();
        m_RotationBehaviour.UpdateBehaviour();
        m_FollowingBehaviour.UpdateBehaviour();
	}

    Vector3 CacheOffset()
	{
        m_CachedOffset = transform.position - m_Target.transform.position;
		return m_CachedOffset;
	}
}
