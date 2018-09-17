using mytest.Interaction;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Изменение объекта в зависимости от расстояния до игрока
    /// </summary>
    public class ActionTrigger_DistortionWithDistance : ActionTrigger, iExitableFromInteractionArea
    {
        [Space(10)]
        public MeshRenderer Renderer;
        [Header("Settings")]
        public float TransitionTime = 1;
        [Header(" - Distortion")]
        public float IdleDistortion = 0.02f;
        public float ActiveDistortion = 0.15f;
        [Header(" - Albedo")]
        public float IdleAlbedo = 0.2f;
        public float ActiveAlbedo = 0.4f;
        [Header(" - Color")]
        public Color MainColor = new Color32(0, 183, 219, 255);

        private Material m_Material;
        private Utils.InterpolationData<float> m_DistLerpData;
        private Utils.InterpolationData<float> m_AlbedoLerpData;

        private const string m_DIST = "_Distortion";
        private const string m_ALBEDO = "_MainTint";
        private const string m_COLOR = "_Color"; 

        protected override void Start()
        {
            base.Start();

            //Создать новый материал
            m_Material = new Material(Renderer.sharedMaterial);
            Renderer.sharedMaterial = m_Material;

            //Задать начальные значения материалу
            m_Material.SetFloat(m_DIST, IdleDistortion);
            m_Material.SetFloat(m_ALBEDO, IdleAlbedo);
            m_Material.SetColor(m_COLOR, MainColor);

            //Данные об анимации
            m_DistLerpData = new Utils.InterpolationData<float>();
            m_AlbedoLerpData = new Utils.InterpolationData<float>();
            m_DistLerpData.TotalTime = TransitionTime;
        }

        public override void Interact()
        {
            m_DistLerpData.From = m_Material.GetFloat(m_DIST);
            m_DistLerpData.To = ActiveDistortion;

            m_AlbedoLerpData.From = m_Material.GetFloat(m_ALBEDO);
            m_AlbedoLerpData.To = ActiveAlbedo;

            m_DistLerpData.Start();
        }

        public void ExitFromInteractableArea()
        {
            m_DistLerpData.From = m_Material.GetFloat(m_DIST);
            m_DistLerpData.To = IdleDistortion;

            m_AlbedoLerpData.From = m_Material.GetFloat(m_ALBEDO);
            m_AlbedoLerpData.To = IdleAlbedo;

            m_DistLerpData.Start();
        }

        void Update()
        {
            if (m_DistLerpData.IsStarted)
            {
                m_DistLerpData.Increment();

                float dist = Mathf.Lerp(m_DistLerpData.From, m_DistLerpData.To, m_DistLerpData.Progress);
                m_Material.SetFloat(m_DIST, dist);

                float albedo = Mathf.Lerp(m_AlbedoLerpData.From, m_AlbedoLerpData.To, m_DistLerpData.Progress);
                m_Material.SetFloat(m_ALBEDO, albedo);

                if (m_DistLerpData.Overtime())
                {
                    m_DistLerpData.Stop();
                }
            }
        }
    }
}
    
