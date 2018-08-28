using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class NewBehaviourScript : MonoBehaviour
{
    Utils.InterpolationData<float> m_LerpData;
    Light light;
    public GameObject ob;

	// Use this for initialization
	void Start ()
    {
        light = GetComponent<Light>();
        m_LerpData = new Utils.InterpolationData<float>(5);
        m_LerpData.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {       
        if (Input.GetKey(KeyCode.L))
        {
            m_LerpData.Start();
        }

        if (Input.GetKey(KeyCode.P))
        {
            Instantiate(ob, transform.position, Quaternion.identity);
        }

        if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment();
            light.intensity = Mathf.Lerp(1, 0, m_LerpData.Progress);

            if (m_LerpData.Overtime())
            {
                m_LerpData.Stop();
            }
        }
	}
}
