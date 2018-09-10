using mytest.Effects;
using mytest.Interaction;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Класс тумблера
    /// Объект, который можно активировать по тапу.
    /// Отслеживает вход/выход зоны взаимодествия
    /// При тапе ведет себе аналогично ActionTrigger. 
    /// </summary>
    public class ActionToggle : ActionToggle_Base
    {
       [Space(10)]
        public MeshRenderer GraphicEmissionRenderer;
        public Effect_Base IdleEffect;
        public Effect_Base[] SelectionEffects;

        private EmissionBehaviour m_EmissionBehaviour;

        public override void ExitFromInteractableArea()
        {
            base.ExitFromInteractableArea();

            //Анимация свечения
            m_EmissionBehaviour = new IdleEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity);

            //Включить пасивный эффект
            IdleEffect.Activate();
        }

        public override void InteractByTap()
        {
            base.InteractByTap();

            //Анимация свечения
            m_EmissionBehaviour = new DisableEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity);
        }

        public override void Interact()
        {
            base.Interact();

            //Выключить пасивный эффект
            IdleEffect.Deactivate();

            //Включить активный эффект 
            for (int i = 0; i < SelectionEffects.Length; i++)
                SelectionEffects[i].Activate();

            //Анимция свечения
            m_EmissionBehaviour = new SelectedEmissionBehaviour(m_EmissionBehaviour.Material, m_EmissionBehaviour.Intensity);
        }


        protected override void Unselect()
        {
            base.Unselect();

            //Выключить активный эффект
            for (int i = 0; i < SelectionEffects.Length; i++)
                SelectionEffects[i].Deactivate();
        }

        protected override void Start()
        {
            base.Start();

            //Создать копию материала и применить ее к объекту
            Material emissionMaterial = new Material(GraphicEmissionRenderer.sharedMaterial);
            GraphicEmissionRenderer.sharedMaterial = emissionMaterial;

            //Анимация свечения
            m_EmissionBehaviour = new IdleEmissionBehaviour(emissionMaterial, 0);

            //Включить пасивный эффект
            IdleEffect.Activate();
        }

        protected override void Update()
        {
            if (GraphicEmissionRenderer == null)
                return;

            m_EmissionBehaviour.Update();

            base.Update();
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
