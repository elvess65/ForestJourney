using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompassBehaviour : MonoBehaviour
{
    private float m_PrevRotation = 0;

	void Update ()
    {
        transform.localRotation = Quaternion.Euler(0, 0, GameManager.Instance.CameraController.transform.localEulerAngles.y);
	}
}
