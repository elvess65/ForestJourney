using System.Collections;
using UnityEngine;

public class CompassWorldBehaviour : MonoBehaviour
{
    public Vector3 Offset;
    public float m_InitScale = 1;
    public float m_Dist = -5;
    public GameObject Graphics;
    public float DissapearTime = 2;
    public float PlayerOffset = 1;

    private Vector3 m_Pos;
    private Quaternion m_Rot;
    private bool m_Init = false;
    private Transform m_Parent;
    private WaitForSeconds m_Delay;

    Utils.InterpolationData<Vector3> m_PosLerpData;
    Utils.InterpolationData<float> m_ScaleLerpData;
    Utils.InterpolationData<Quaternion> m_RotLerpData;

	void Start ()
    {
        m_Delay = new WaitForSeconds(DissapearTime);
        Graphics.SetActive(false);
        m_Parent = transform.parent;
        InputManager.Instance.OnInputStateChange += InputStateChangedHandler;
    }

    public void Show()
    {
        transform.parent = m_Parent;
        Graphics.SetActive(true);

        transform.localPosition = m_Pos;
        transform.localRotation = m_Rot;
        transform.localScale = new Vector3(m_InitScale, m_InitScale, 1);
    }

    public void Animate()
    {
        m_PosLerpData = new Utils.InterpolationData<Vector3>(1);
        m_PosLerpData.From = transform.position;
        m_PosLerpData.To = new Vector3(GameManager.Instance.GameState.Player.transform.position.x, GameManager.Instance.GameState.Player.transform.position.y + PlayerOffset, GameManager.Instance.GameState.Player.transform.position.z);

        m_ScaleLerpData = new Utils.InterpolationData<float>(1);
        m_ScaleLerpData.From = transform.localScale.x;
        m_ScaleLerpData.To = 2;

        m_RotLerpData = new Utils.InterpolationData<Quaternion>(1);
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

    IEnumerator WaitDissapearTime()
    {
        yield return m_Delay;

        Graphics.SetActive(false);
    }

	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
            Show();

        if (Input.GetKeyDown(KeyCode.P))
            Animate();

        if (m_PosLerpData.IsStarted)
        {
            m_PosLerpData.Increment();
            transform.position = Vector3.Lerp(m_PosLerpData.From, m_PosLerpData.To, m_PosLerpData.Progress);
            float scale = Mathf.Lerp(m_ScaleLerpData.From, m_ScaleLerpData.To, m_PosLerpData.Progress);
            transform.localScale = new Vector3(scale, scale, 1);

            transform.rotation = Quaternion.Lerp(m_RotLerpData.From, m_RotLerpData.To, m_PosLerpData.Progress);

            if (m_PosLerpData.Overtime())
            {
                transform.parent = null;
                StartCoroutine(WaitDissapearTime());
                m_PosLerpData.Stop();
            }
        }
        
	}
}
