using UnityEngine;

public class PingPongObject : MonoBehaviour
{
    [Header("Static")]
    public float Amplitude = 1;
    public float Frequency = 1;
    [Header("Random")]
    public bool Randomize = true;
    public float AmplitudeMin = 0.1f;
    public float AmplitudeMax = 1;
    public float FrequencyMin = 0.1f;
    public float FrequencyMax = 1;

    private float m_InitPos;

    private void Start()
    {
        m_InitPos = transform.position.y;

        if (Randomize)
        {
            Amplitude = Random.Range(AmplitudeMin, AmplitudeMax);
            Frequency = Random.Range(FrequencyMin, FrequencyMax);
        }
    }

    void Update ()
    {
        float yPos = Mathf.Sin(Time.time * Frequency) * Amplitude + m_InitPos;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
	}
}
