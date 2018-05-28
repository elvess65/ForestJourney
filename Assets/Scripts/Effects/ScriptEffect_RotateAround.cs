using UnityEngine;

public class ScriptEffect_RotateAround : MonoBehaviour
{
    public float RotationSpeed = 20;

	void Update ()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * RotationSpeed);
	}
}
