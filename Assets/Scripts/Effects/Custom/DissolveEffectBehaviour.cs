using mytest.Utils;
using UnityEngine;

namespace mytest.Effects.Custom
{
    /// <summary>
    /// Накладывает на список MeshRenderers эффект Dissolve (прячет объекты)
    /// </summary>
    public class DissolveEffectBehaviour : MonoBehaviour
    {
        public System.Action OnDissolveFinished;

        public float DissolveTime = 3;
        public float DissolveValue = 0.5f;
        public MeshRenderer[] MeshRenderers;

        private Material m_Material;
        private InterpolationData<float> m_LerpData;
        private const string m_MATERIAL_DISSOLVE_NAME = "_SliceAmount";

        void Start()
        {
            m_Material = new Material(MeshRenderers[0].sharedMaterial);
            for (int i = 0; i < MeshRenderers.Length; i++)
                MeshRenderers[i].sharedMaterial = m_Material;

            m_LerpData = new InterpolationData<float>();
            m_LerpData.From = 0;
            m_LerpData.To = DissolveValue;
            m_LerpData.TotalTime = DissolveTime;
        }

        public void Dissolve()
        {
            m_LerpData.Start();
        }

        void Update()
        {
            if (m_LerpData.IsStarted)
            {
                m_LerpData.Increment();
                float dissolve = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
                m_Material.SetFloat(m_MATERIAL_DISSOLVE_NAME, dissolve);

                if (m_LerpData.Overtime())
                {
                    m_LerpData.Stop();

                    if (OnDissolveFinished != null)
                        OnDissolveFinished();
                }
            }
        }
    }
}