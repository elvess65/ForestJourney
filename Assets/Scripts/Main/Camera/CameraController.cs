﻿using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public System.Action OnCameraArrived;

    private System.Action m_OnFocusDelay;

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
	[SerializeField]
	private float m_CurSmoothFactor;
    [SerializeField]
    private bool m_LookAtPlayer = false;
	[SerializeField]
    private bool m_InitFocusingAtTarget = false;
	private bool m_IsRotating = false;

    //Focus
    private bool m_IsFocusingAtSomething = false;
    //Focus some time at
    private float m_FocusingTime;
    private Transform m_FocusingTarget;
	private System.Action m_OnFocusingFinished;

    public void Init(Transform target)
    {
        m_Target = target;
        m_CurSmoothFactor = SmoothFactor_Following;
        m_CurCameraOffset = transform.position - m_Target.transform.position;

        StartInitFocusing();
    }

	public AnimationCurve Curve;
	float degrees = 0;
	float percent;
	void LateUpdate()
	{
		if (!GameManager.Instance.IsActive && m_Target == null)
			return;

        if (!m_IsRotating)
        {
            InitFocusing();

            SetCameraRotation();
            SetSmoothSpeed();
            SetCameraPosition();

            if (m_LookAtPlayer)
                transform.LookAt(m_Target);
        }
        else 
        {
			degrees += 10 * Curve.Evaluate(percent) * Time.deltaTime;
			percent = degrees / 90;
			transform.LookAt(m_Target);
			Debug.Log(Time.time + " " + degrees + " " + percent + " " + Curve.Evaluate(percent));
			//transform.LookAt(targetObject.transform);

			transform.RotateAround(m_Target.transform.position, Vector3.up, 10 * Curve.Evaluate(percent) * Time.deltaTime);

            if (percent > 1)
            {
                m_CurCameraOffset = transform.position - m_Target.transform.position;
                m_IsRotating = false;
            }
        }
	}


    //Focus
    /// <summary>
    /// Фокусируеться некоторое время на каком-то объекте, а затем возвращает фокус на игрока
    /// </summary>
    /// <param name="target">Объект, который нужно показать</param>
    /// <param name="focusingTime">Время, на которое объект будет сфокусен</param>
    /// <param name="onFocusingFinished">События окончания всего фокуса (Возврат фокуса на игрока)</param>
    public void FocusSomeTimeAt(Transform target, float focusingTime, System.Action onFocusingFinished, System.Action onFocusDelayFinished)
    {
        m_FocusingTarget = target;
        m_FocusingTime = focusingTime;
        m_OnFocusingFinished = onFocusingFinished;

        OnCameraArrived += WaitDelay;
        m_OnFocusDelay += onFocusDelayFinished;
        FocusAt(target);
    }

    void WaitDelay()
    {
        StartCoroutine(FocusingSomeTimeAt());
    }

	IEnumerator FocusingSomeTimeAt()
	{
		yield return new WaitForSeconds(m_FocusingTime);

        if (m_OnFocusDelay != null)
        {
            m_OnFocusDelay();
            m_OnFocusDelay = null;
        }

        OnCameraArrived += m_OnFocusingFinished;
        FocusAt(GameManager.Instance.GameState.Player.transform);
	}

    void FocusAt(Transform target)
	{
        OnCameraArrived += FocusingFinished;
		m_Target = target;

		StartFocusing();
	}

    /// <summary>
    /// Начало фокуса (Изменение скорости камеры)
    /// </summary>
    void StartFocusing()
    {
        m_IsFocusingAtSomething = true;
        m_CurSmoothFactor = SmoothFactor_FocusingAtOther;
    }

    /// <summary>
    /// Окончания фокуса (Изменение скорости камеры)
    /// </summary>
    void FocusingFinished()
    {
        m_IsFocusingAtSomething = false;
    }


    //Rotate
    public void RotateCamera(bool lockInput = false, bool clockWise = true, float angleSpeed = 405, float rotationTime = 1)
    {
        if (lockInput)
        {
            InputManager.Instance.InputIsEnabled = false;
            OnCameraArrived += InputManager.Instance.UnlockInput;
        }

		PostProcessingController.Instance.DecreaseSaturation();

		m_CurRotationSpeed = Time.deltaTime * angleSpeed;
        if (!clockWise)
            m_CurRotationSpeed *= -1;
        
        m_LookAtPlayer = true;
        m_IsRotating = true;

        //TODO iTween 

        OnCameraArrived += RotationFinished;
        StartCoroutine(RotateSomeTime(rotationTime));
    }

	IEnumerator RotateSomeTime(float time)
	{
		WaitForSeconds waitDelay = new WaitForSeconds(time);
		yield return waitDelay;

		m_CurRotationSpeed = 0;
	}

    void RotationFinished()
    {
        StopLookAtPlayer();
        m_IsRotating = false;
    }


	/// <summary>
    /// Вращение камеры (Изменяет m_CurSmoothFactor)
	/// </summary>
	void SetCameraRotation()
	{
		//Angle and offset
		if (!m_CurRotationSpeed.Equals(0f)) //Если вращаеться - начать учитывать вращение и задать скорость
		{
			Quaternion camTurnAngle = Quaternion.AngleAxis(m_CurRotationSpeed, Vector3.up);
			m_CurCameraOffset = camTurnAngle * m_CurCameraOffset;

			m_CurSmoothFactor = SmoothFactor_Rotating;
		}
	}

    /// <summary>
    /// Изменение скорости камеры, если ничего не происходит
    /// </summary>
    void SetSmoothSpeed()
    {
        if (m_CurRotationSpeed.Equals(0f) && !m_IsFocusingAtSomething && !m_IsRotating)//Если камера не вращаеться - задать скорость
		{
            m_CurSmoothFactor = SmoothFactor_Following;
        }
    }

	/// <summary>
    /// Движение камеры (Использует m_CurSmoothFactor)
	/// </summary>
	void SetCameraPosition()
    {
		Vector3 newPos = m_Target.transform.position + m_CurCameraOffset;
		transform.position = Vector3.Slerp(transform.position, newPos, m_CurSmoothFactor);

        if (m_CurRotationSpeed.Equals(0f))
        {
            float sqrDistToNewPos = (newPos - transform.position).sqrMagnitude;
            if (sqrDistToNewPos <= 0.001f)
            {
                if (OnCameraArrived != null)
                {
                    OnCameraArrived();
                    OnCameraArrived = null;
                }
            }
        }
	}


    /// <summary>
    /// Начинает возврат поворота камеры в позицию фокуса на персонаже
    /// </summary>
    void StartInitFocusing()
    {
        m_LookAtPlayer = true;
        m_InitFocusingAtTarget = true;

        OnCameraArrived += StopInitFocusing;
    }

    void StopInitFocusing()
    {
        StopLookAtPlayer();
        m_InitFocusingAtTarget = false;
    }

    void StopLookAtPlayer()
    {
        m_LookAtPlayer = false;
    }


    /// <summary>
    /// Изначальная фокусировка на цели (из позиции камеры на старте игры на игрока)
    /// </summary>
    void InitFocusing()
    {
        //Интерполяция от текущего оффсета к нужному (выключаеться автоматически)
		if (m_InitFocusingAtTarget && (m_CurCameraOffset - CameraOffset).sqrMagnitude >= 0.01f)
			m_CurCameraOffset = Vector3.Slerp(m_CurCameraOffset, CameraOffset, SmoothFactor_InitFocusing);
    }
}