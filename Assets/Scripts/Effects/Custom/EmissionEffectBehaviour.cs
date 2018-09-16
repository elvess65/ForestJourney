using System.Collections.Generic;
using UnityEngine;

namespace mytest.Effects.Custom
{
    public class EmissionEffectBehaviour : MonoBehaviour
    {
        public int GroupID = 0;
        public MeshRenderer GraphicEmissionRenderer;
        public Effect_Base[] DisableEffects;
        [Header("Timing Settings")]
        [MinMaxRangeSlider.MinMax(0, 10)]
        public MinMaxRangeSlider.MinMaxPair IdleSmoothTime = new MinMaxRangeSlider.MinMaxPair(0.5f, 2f);
        [MinMaxRangeSlider.MinMax(0, 10)]
        public MinMaxRangeSlider.MinMaxPair SelectedSmoothTime = new MinMaxRangeSlider.MinMaxPair(1.0f, 1.5f);
        [MinMaxRangeSlider.MinMax(0, 10)]
        public MinMaxRangeSlider.MinMaxPair DisableSmoothTime = new MinMaxRangeSlider.MinMaxPair(5f, 5f);

        private EmissionBehaviour m_EmissionBehaviour;
        private Material m_EmissionMaterial;
        private EmissionAnimation m_Data;

        private static Dictionary<int, EmissionAnimation> m_GROUP_EMISSION_DATA;

        void Start()
        {
            //Тайминги для групы (для каждой групы одинаковые тайминги)
            if (m_GROUP_EMISSION_DATA == null)
                m_GROUP_EMISSION_DATA = new Dictionary<int, EmissionAnimation>();

            
            //Если еще не созданы данные для групы - создать
            if (!m_GROUP_EMISSION_DATA.ContainsKey(GroupID))
            {
                m_Data = new EmissionAnimation(Random.Range(IdleSmoothTime.Min, IdleSmoothTime.Max),
                                             Random.Range(SelectedSmoothTime.Min, SelectedSmoothTime.Max),
                                             Random.Range(DisableSmoothTime.Min, DisableSmoothTime.Max));

                m_GROUP_EMISSION_DATA.Add(GroupID, m_Data);
            }
            else //Если уже созданы данные для групы - получить
                m_Data = m_GROUP_EMISSION_DATA[GroupID]; 

            //Создать копию материала и применить ее к объекту
            m_EmissionMaterial = new Material(GraphicEmissionRenderer.sharedMaterial);
			GraphicEmissionRenderer.sharedMaterial = m_EmissionMaterial;

            SetIdle();
        }

		void Update()
		{
			if (m_EmissionBehaviour != null)
				m_EmissionBehaviour.Update();
		}


        public void SetIdle()
        {
            m_EmissionBehaviour = new IdleEmissionBehaviour(m_EmissionBehaviour == null ? m_EmissionMaterial : m_EmissionBehaviour.Material,
                                                            m_EmissionBehaviour == null ? 0                  : m_EmissionBehaviour.Intensity,
                                                            m_Data.IdleSmoothTime);
        }

        public void SetMax()
        {
            m_EmissionBehaviour = new SelectedEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity, m_Data.SelectedSmoothTime);
        }

        public void SetDisable()
        {
            m_EmissionBehaviour = new DisableEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity, m_Data.DisableSmoothTime);

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

			public EmissionBehaviour(Material mat)
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

			public IdleEmissionBehaviour(Material mat, float curIntensity, float smoothTime) : base(mat)
			{
				m_CurTime = 0;

				if (curIntensity >= 0)
					StartSmoothAnimation(curIntensity, m_INTENSITY_VALUE, smoothTime);
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

            public SelectedEmissionBehaviour(Material mat, float curIntensity, float smoothTime) : base(mat)
			{
				StartSmoothAnimation(curIntensity, m_INTENSITY_VALUE, smoothTime);
			}

			protected override float GetIntensity()
			{
				return m_INTENSITY_VALUE;
			}
		}

		class DisableEmissionBehaviour : EmissionBehaviour
		{
			private const float m_INTENSITY_VALUE = -1;

			public DisableEmissionBehaviour(Material mat, float curIntensity, float smoothTime) : base(mat)
			{
				m_Intensity = curIntensity;

				StartSmoothAnimation(m_Intensity, m_INTENSITY_VALUE, smoothTime);
			}

			protected override float GetIntensity()
			{
				return m_Intensity;
			}
		}


        struct EmissionAnimation
        {
            public float IdleSmoothTime;
            public float SelectedSmoothTime;
            public float DisableSmoothTime;

            public EmissionAnimation(float idleSmoothTime, float selectedSmoothTime, float disableSmoothTime)
            {
                IdleSmoothTime = idleSmoothTime;
                SelectedSmoothTime = selectedSmoothTime;
                DisableSmoothTime = disableSmoothTime;
            }
        }
    }
}
