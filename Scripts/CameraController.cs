using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    public float RotationsSpeed = 5.0f;
    public Vector3 CameraOffset;

    private Transform m_Target;
    private float m_CurRotationSpeed = 0;
    [SerializeField]
    private Vector3 m_CamOffset;
    private bool m_InitFocus = false;

    public void Init(Transform target)
    {
        m_Target = target;
        m_CamOffset = transform.position - m_Target.transform.position;
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Rotate Camera"))
            RotateRandomly();
    }

    public void RotateRandomly()
    {
        int dir = Random.Range(0, 100) > 50 ? -1 : 1;
        m_CurRotationSpeed = Random.Range(0.5f, 2f) * dir;

        StartCoroutine(RotatePeriod(Random.Range(0.5f, 2f)));
    }

    IEnumerator RotatePeriod(float time)
    {
        yield return new WaitForSeconds(time);
        m_CurRotationSpeed = 0;
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.IsActive && m_Target == null)
            return;

        if (!m_InitFocus && (m_CamOffset - CameraOffset).sqrMagnitude >= 0.1f)
            m_CamOffset = Vector3.Slerp(m_CamOffset, CameraOffset, SmoothFactor);
        else
            m_InitFocus = true;

        //Angle and offset
        Quaternion camTurnAngle = Quaternion.AngleAxis(m_CurRotationSpeed, Vector3.up);
        m_CamOffset = camTurnAngle * m_CamOffset;

        //Slerp to offset
        Vector3 newPos = m_Target.transform.position + m_CamOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        //Slerp to rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(m_Target.transform.position - transform.position), Time.deltaTime);
        //transform.rotation = Quaternion.LookRotation(Player.transform.position - transform.position);
    }
}
