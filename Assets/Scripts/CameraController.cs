using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform Player;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    public float RotationsSpeed = 5.0f;
    public Vector3 CameraOffset;

    private Vector3 m_CameraOffset;
    private bool m = false;

    private void Start()
    {
        m_CameraOffset = transform.position - Player.transform.position;
        m_CameraOffset = CameraOffset;
    }

    void LateUpdate()
    {
        if (!m && (m_CameraOffset - CameraOffset).sqrMagnitude >= 0.1f)
        {
            m_CameraOffset = Vector3.Slerp(m_CameraOffset, CameraOffset, Time.deltaTime);
            Debug.Log((m_CameraOffset - CameraOffset).sqrMagnitude);
        }
        else
            m = true;

        Quaternion camTurnAngle = Quaternion.AngleAxis(RotationsSpeed, Vector3.up);
        m_CameraOffset = camTurnAngle * m_CameraOffset;

        Vector3 newPos = Player.transform.position + m_CameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);

        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Player.transform.position - transform.position), Time.deltaTime);
        transform.LookAt(Player.transform);
    }
}
