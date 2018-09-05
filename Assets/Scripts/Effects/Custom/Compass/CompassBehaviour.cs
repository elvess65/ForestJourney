using UnityEngine;

public class CompassBehaviour : MonoBehaviour
{
    public Transform Graphics;

    private float m_PrevRotation = 0;

	void LateUpdate ()
    {
        Graphics.transform.localRotation = Quaternion.Euler(0, 0, GameManager.Instance.CameraController.transform.localEulerAngles.y);
	}
}
