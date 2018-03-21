using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public PlayerController Player;
    private Vector3 dir;

	void Update () 
    {
        if (Input.GetKey(KeyCode.R))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            Vector3 tDir = new Vector3(x, 0, y);

            if (tDir != Vector3.zero)
                dir = tDir;

            Player.MoveInDir(dir.normalized * 10);
        }

        float rot = Input.GetAxis("Horizontal");
        //Camera.main.transform.LookAt(Player.transform);
        //Camera.main.transform.RotateAround(transform.position, Vector3.up, rot);
        //Camera.main.transform.Translate(Vector3.right * Time.deltaTime);
	}
}
