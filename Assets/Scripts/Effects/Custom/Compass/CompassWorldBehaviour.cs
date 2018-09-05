using mytest.UI.InputSystem;
using mytest.Utils;
using System.Collections;
using UnityEngine;

public class CompassWorldBehaviour : MonoBehaviour
{
    [Tooltip("Отступ от позиции на экране")]
    public Vector3 Offset;
    [Tooltip("Начальный размер (при появлении на экране)")]
    public float m_InitScale = 1;
    [Tooltip("Расстояние от игрока к камере")]
    public float m_Dist = -5;
    public GameObject Graphics;
    [Tooltip("Отступ по высоте от позиции игрока")]
    public float PlayerOffset = 1;
    [Tooltip("расстояние до отключения")]
    public float DistanceToDisable = 5;

    private Vector3 m_Pos;
    private Quaternion m_Rot;
    private bool m_Init = false;
    private Transform m_Parent;
    private bool m_IsArrived = false;

    private InterpolationData<Vector3> m_PosLerpData;
    private InterpolationData<float> m_ScaleLerpData;
    private InterpolationData<Quaternion> m_RotLerpData;

	void Start ()
    {
        m_Parent = transform.parent;

        Graphics.SetActive(false);

        //Инициализация при первом включении ввода
        InputManager.Instance.OnInputStateChange += InputStateChangedHandler;
    }

    public void Show()
    {
        if (m_IsArrived)
            m_IsArrived = false;

        transform.parent = m_Parent;

        ShowObject();

        //Задать кешированные локальные данные
        transform.localPosition = m_Pos;
        transform.localRotation = m_Rot;
        transform.localScale = new Vector3(m_InitScale, m_InitScale, 1);
    }

    public void Animate()
    {
        //Перемещение
        m_PosLerpData = new InterpolationData<Vector3>(1);
        m_PosLerpData.From = transform.position;
        m_PosLerpData.To = new Vector3(GameManager.Instance.GameState.Player.transform.position.x, GameManager.Instance.GameState.Player.transform.position.y + PlayerOffset, GameManager.Instance.GameState.Player.transform.position.z);

        //Размер
        m_ScaleLerpData = new InterpolationData<float>(1);
        m_ScaleLerpData.From = transform.localScale.x;
        m_ScaleLerpData.To = 2;

        //Вращение
        m_RotLerpData = new InterpolationData<Quaternion>(1);
        m_RotLerpData.From = transform.rotation;
        m_RotLerpData.To = Quaternion.Euler(new Vector3(90, 0, 0));

        m_PosLerpData.Start();
    }


    void InputStateChangedHandler(bool state)
    {
        InputManager.Instance.OnInputStateChange -= InputStateChangedHandler;

        Init();
    }

    void Init()
    {
        Vector3 anchorPos = GameManager.Instance.GameState.Player.transform.position;
        Vector3 fromAnchorToCam = anchorPos - GameManager.Instance.CameraController.transform.position;

        //Начальная позиция
        transform.position = anchorPos;

        //Вращение в сторону камеры
        transform.rotation = Quaternion.LookRotation(fromAnchorToCam);

        //Размер
        transform.localScale = new Vector3(m_InitScale, m_InitScale, 1);

        //Окончательная позиция
        transform.position = transform.position + fromAnchorToCam.normalized * m_Dist - Offset;

        //Локальные параметры
        m_Pos = transform.localPosition;
        m_Rot = transform.localRotation;
        m_InitScale = transform.localScale.x;

        m_Init = true;
    }

    void ShowObject()
    {
        Graphics.SetActive(true);
    }

    void HideObject()
    {
        Graphics.SetActive(false);
    }


    void Update ()
    {
        if (m_PosLerpData.IsStarted)
        {
            m_PosLerpData.Increment();

            //Перемещение
            transform.position = Vector3.Lerp(m_PosLerpData.From, m_PosLerpData.To, m_PosLerpData.Progress);

            //Размер
            float scale = Mathf.Lerp(m_ScaleLerpData.From, m_ScaleLerpData.To, m_PosLerpData.Progress);
            transform.localScale = new Vector3(scale, scale, 1);

            //Вращение
            transform.rotation = Quaternion.Lerp(m_RotLerpData.From, m_RotLerpData.To, m_PosLerpData.Progress);

            if (m_PosLerpData.Overtime())
            {
                transform.parent = null;
                m_PosLerpData.Stop();

                m_IsArrived = true;
            }
        }

        //Просчет расстояния до выключения
        if (m_IsArrived)
        {
            Vector3 from = transform.position;
            Vector3 to = GameManager.Instance.GameState.Player.transform.position;

            if ((to - from).sqrMagnitude >= DistanceToDisable * DistanceToDisable)
            {
                m_IsArrived = false;

                HideObject();
            }
        }
	}

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, DistanceToDisable);
    }
}
