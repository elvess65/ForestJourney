using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public float Magnitude = 1;
    public float Frequency = 1;

    private float UpVelocity = 0;
    private float Gravity = 9.8f;
    private bool AffectGravity = false;

	void Update ()
    {
        if (AffectGravity)
            UpVelocity -= Gravity * Time.deltaTime;

        Vector3 v = transform.position;
        v.y += UpVelocity * Time.deltaTime;
        transform.position = v;

        if (AffectGravity && v.y <= 0)
        {
            AffectGravity = false;
            UpVelocity = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpVelocity = 10;

            AffectGravity = true;
        }
	}
}
