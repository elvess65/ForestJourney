using UnityEngine;

public class DissolveEffectBehaviour : MonoBehaviour 
{
    public System.Action OnDissolveFinished;

    public float DissolveTime = 3;
    public float DissolveValue = 0.5f;
    public MeshRenderer[] MeshRenderers;

    private Material m_Material;
    private Utils.InterpolationData<float> m_LerpData;

	void Start () 
    {
        m_Material = new Material(MeshRenderers[0].sharedMaterial);
        for (int i = 0; i < MeshRenderers.Length; i++)
            MeshRenderers[i].sharedMaterial = m_Material;

        m_LerpData = new Utils.InterpolationData<float>();
        m_LerpData.From = 0;
        m_LerpData.To = DissolveValue;
        m_LerpData.TotalTime = DissolveTime;
	}
	
    public void Dissolve()
    {
        m_LerpData.Start();
    }

	void Update () 
    {
        if (m_LerpData.IsStarted)
        {
            m_LerpData.Increment(Time.deltaTime);
            float dissolve = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
            m_Material.SetFloat("_SliceAmount", dissolve);

            if (m_LerpData.Overtime())
            {
                m_LerpData.Stop();

                if (OnDissolveFinished != null)
                    OnDissolveFinished();
            }
        }
	}
}
 