using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public Vector3 CameraOffset;

    [Header("Smooth factors")]
	[Range(0.01f, 1.0f)]
	public float SmoothFactor_InitFocusing = 0.5f;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor_Following = 0.5f;
	[Range(0.01f, 1.0f)]
	public float SmoothFactor_Rotating = 0.5f;
	[Range(0.01f, 1.0f)]
	public float SmoothFactor_FocusingAtOther = 0.5f;

    private Transform m_Target;
    [Header("DEBUG")]
	[SerializeField]
    private Vector3 m_CurCameraOffset;
    [SerializeField]
    private float m_CurRotationSpeed = 0;
    private float m_TargetRotationSpeed = 0;
    [SerializeField]
    private bool m_LookAtPlayer = false;
	[SerializeField]
    private bool m_InitFocusingAtTarget = false;
    public Transform T;

    public void Init(Transform target)
    {
        m_Target = target;
        m_CurSmoothFactor = SmoothFactor_Following;
        m_CurCameraOffset = transform.position - m_Target.transform.position;

        ResetRotation();
    }

    public void FocusAt(Transform target)
    {
        //m_CurSmoothFactor = SmoothFactor_FocusingAtOther;
        //Debug.LogError(m_CurSmoothFactor);
        m_Target = target;
        //onCameraArrived += () => m_CurSmoothFactor = SmoothFactor_Following;
    }

    public void FocusSomeTimeAt(Transform target, float delayBedoreFocusing, float focusingTime, System.Action onFocusingFinished)
    {
        StartCoroutine(FocusingSomeTimeAt(target, delayBedoreFocusing, focusingTime, onFocusingFinished));
    }

    public void RotateRandomly()
    {
        int dir = Random.Range(0, 100) > 50 ? -1 : 1;
        m_CurRotationSpeed = Time.deltaTime * 45f;//Random.Range(0.5f, 2f) * dir;
        m_LookAtPlayer = true;

        startTime = Time.time;
        lerpRotSpeed = true;

        StartCoroutine(RotateSomeTime(1));//Random.Range(0.5f, 2f)));
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 100), "Rotate"))
            RotateRandomly();

        if (GUI.Button(new Rect(10, 200, 150, 100), "FocusAtObject"))
            FocusAt(T);

        if (GUI.Button(new Rect(10, 400, 150, 100), "FocusAtPlayert"))
            FocusAt(GameManager.Instance.GameState.Player.transform);
    }

    IEnumerator FocusingSomeTimeAt(Transform target, float delay, float time, System.Action onFocusingFinished)
    {
        yield return new WaitForSeconds(delay);
        FocusAt(target);
        yield return new WaitForSeconds(time);

        if (onFocusingFinished != null)
            onFocusingFinished();
    }

    IEnumerator RotateSomeTime(float time)
    {
        WaitForSeconds waitDelay = new WaitForSeconds(time);

        yield return waitDelay;
        m_CurRotationSpeed = 0;
        lerpRotSpeed = false;
        Debug.LogWarning("Stop rotating");

        yield return waitDelay;
        m_LookAtPlayer = false;
    }
    bool lerpRotSpeed = false;
	private float startTime;
    [SerializeField]
    private float m_CurSmoothFactor;

    void LateUpdate()
    {
        if (!GameManager.Instance.IsActive && m_Target == null)
            return;

        if (lerpRotSpeed)
        {
			//float t = (Time.time - startTime) / 2;
            //m_CurRotationSpeed = Mathf.SmoothStep(0, m_TargetRotationSpeed, t);
        }
                                      
        InitFocusing();

        //Angle and offset
        if (!m_CurRotationSpeed.Equals(0f))
        {
            Quaternion camTurnAngle = Quaternion.AngleAxis(m_CurRotationSpeed, Vector3.up);
            m_CurCameraOffset = camTurnAngle * m_CurCameraOffset;

            m_CurSmoothFactor = SmoothFactor_Rotating;
        }
        else
			m_CurSmoothFactor = SmoothFactor_Following;
        
        SetCameraPosition(m_CurSmoothFactor);
        
        if (m_LookAtPlayer)
            transform.LookAt(m_Target);
    }

    /// <summary>
    /// Движение камеры
    /// </summary>
    /// <param name="smoothFactor">Smooth factor.</param>
    void SetCameraPosition(float smoothFactor)
    {
		Vector3 newPos = m_Target.transform.position + m_CurCameraOffset;
		transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        float sqrDistToNewPos = (newPos - transform.position).sqrMagnitude;
        Debug.Log(sqrDistToNewPos);
        if (sqrDistToNewPos <= 0.001f)
        {
            if (onCameraArrived != null)
            {
                onCameraArrived();
                onCameraArrived = null;
            }
            Debug.LogWarning("onCameraArrived");
        }
	}

    System.Action onCameraArrived;

    /// <summary>
    /// Начинает возврат поворота камеры в изначальную позицию
    /// </summary>
    void ResetRotation()
    {
        m_LookAtPlayer = true;
        m_InitFocusingAtTarget = true;
    }

    /// <summary>
    /// Изначальная фокусировка на цели (из позиции камеры на старте игры на игрока)
    /// </summary>
    void InitFocusing()
    {
		if (m_InitFocusingAtTarget && (m_CurCameraOffset - CameraOffset).sqrMagnitude >= 0.1f)
			m_CurCameraOffset = Vector3.Slerp(m_CurCameraOffset, CameraOffset, SmoothFactor_InitFocusing);
		else
		{
            if (m_InitFocusingAtTarget)
            {
                m_InitFocusingAtTarget = false;
                onCameraArrived += () => m_LookAtPlayer = false;
                //m_LookAtPlayer = false;

                Debug.LogWarning("FINISH FOCUSING");
                //TODO: Add event
            }
		}
    }
}
