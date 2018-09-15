using System.Collections;
using UnityEngine;

namespace mytest.Effects.Custom
{
    public class EmissionEffectBehaviour : MonoBehaviour
    {
        public MeshRenderer GraphicEmissionRenderer;
        public Effect_Base[] DisableEffects;

        private EmissionBehaviour m_EmissionBehaviour;
        private Material m_EmissionMaterial;

        void Start()
        {
			//Создать копию материала и применить ее к объекту
			m_EmissionMaterial = new Material(GraphicEmissionRenderer.sharedMaterial);
			GraphicEmissionRenderer.sharedMaterial = m_EmissionMaterial;

            SetIdle();
        }

		void Update()
		{
			if (m_EmissionBehaviour != null)
				m_EmissionBehaviour.Update();

            if (Input.GetKeyDown(KeyCode.M))
                SetDisable();
		}

        public void SetIdle()
        {
            m_EmissionBehaviour = new IdleEmissionBehaviour(m_EmissionBehaviour == null ? m_EmissionMaterial : m_EmissionBehaviour.Material, 0);
        }

        public void SetMax()
        {
			m_EmissionBehaviour = new SelectedEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity);
        }

        public void SetDisable()
        {
			m_EmissionBehaviour = new DisableEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity);

            for (int i = 0; i < DisableEffects.Length; i++)
                DisableEffects[i].Activate();
        }


		abstract class EmissionBehaviour
		{
			protected float m_Intensity;
			protected Utils.InterpolationData<float> m_LerpData;

			protected Material m_Material;
			protected Color m_BaseColor;
			protected const string m_EMISSION_COLOR_NAME = "_EmissionColor";

			public float Intensity
			{
				get { return m_Intensity; }
			}
			public Material Material
			{
				get { return m_Material; }
			}

			public EmissionBehaviour(Material mat, float curIntensity)
			{
				m_Material = mat;
				m_BaseColor = Color.white;
			}

			public void Update()
			{
				//Плавная анимация к начальной позиции 
				if (m_LerpData.IsStarted)
				{
					m_LerpData.Increment();
					m_Intensity = Mathf.Lerp(m_LerpData.From, m_LerpData.To, m_LerpData.Progress);
					if (m_LerpData.Overtime())
						m_LerpData.Stop();

				}
				else
					m_Intensity = GetIntensity();

				SetColor();
			}

			protected void StartSmoothAnimation(float fromIntensity, float toIntensity, float totalTime)
			{
				m_LerpData = new Utils.InterpolationData<float>();
				m_LerpData.From = fromIntensity;
				m_LerpData.To = toIntensity;
				m_LerpData.TotalTime = totalTime;
				m_LerpData.Start();
			}

			protected abstract float GetIntensity();

			void SetColor()
			{
				Color color = m_BaseColor * Mathf.LinearToGammaSpace(m_Intensity);
				m_Material.SetColor(m_EMISSION_COLOR_NAME, color);
			}
		}

		class IdleEmissionBehaviour : EmissionBehaviour
		{
			private float m_CurTime = 0;
			private const float m_INTENSITY_VALUE = 1;
			private const float m_SMOOTH_TIME = 1;

			public IdleEmissionBehaviour(Material mat, float curIntensity) : base(mat, curIntensity)
			{
				m_CurTime = 0;

				if (curIntensity >= 0)
					StartSmoothAnimation(curIntensity, m_INTENSITY_VALUE, m_SMOOTH_TIME);
			}

			protected override float GetIntensity()
			{
				m_CurTime += Time.deltaTime;
				return Mathf.PingPong(m_CurTime, m_INTENSITY_VALUE);
			}
		}

		class SelectedEmissionBehaviour : EmissionBehaviour
		{
			private const float m_INTENSITY_VALUE = 5;
			private const float m_SMOOTH_TIME = 1;

			public SelectedEmissionBehaviour(Material mat, float curIntensity) : base(mat, curIntensity)
			{
				StartSmoothAnimation(curIntensity, m_INTENSITY_VALUE, m_SMOOTH_TIME);
			}

			protected override float GetIntensity()
			{
				return m_INTENSITY_VALUE;
			}
		}

		class DisableEmissionBehaviour : EmissionBehaviour
		{
			private const float m_INTENSITY_VALUE = -1;
			private const float m_SMOOTH_TIME = 5;

			public DisableEmissionBehaviour(Material mat, float curIntensity) : base(mat, curIntensity)
			{
				m_Intensity = curIntensity;

				StartSmoothAnimation(m_Intensity, m_INTENSITY_VALUE, m_SMOOTH_TIME);
			}

			protected override float GetIntensity()
			{
				return m_Intensity;
			}
		}
    }
}
