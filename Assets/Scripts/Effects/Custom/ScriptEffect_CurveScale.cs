using UnityEngine;

namespace mytest.Effects.Custom
{
    /// <summary>
    /// Изменение размера объекта согласно кривой
    /// </summary>
    public class ScriptEffect_CurveScale : MonoBehaviour
    {
        public float TotalTime = 1;
        public AnimationCurve Curve;

        private float m_CurTime;
        private bool m_IsActive = false;
        private Vector3 m_InitScale;

        public void Play()
        {
            if (m_IsActive)
                return;

            m_InitScale = transform.localScale;
            m_CurTime = 0;
            m_IsActive = true;
        }

        void Update()
        {
            if (m_IsActive)
            {
                m_CurTime += Time.deltaTime;
                transform.localScale = m_InitScale * Curve.Evaluate(m_CurTime / TotalTime);
 
                if (m_CurTime >= TotalTime)
                {
                    m_IsActive = false;
                    transform.localScale = m_InitScale;
                }
            }
        }
    }
}
