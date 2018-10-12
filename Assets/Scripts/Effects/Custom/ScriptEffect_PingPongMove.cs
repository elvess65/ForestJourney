using UnityEngine;

namespace mytest.Effects.Custom
{
    /// <summary>
    /// Изменение положения объекта согласно указаному вектору кривой
    /// </summary>
    public class ScriptEffect_PingPongMove : MonoBehaviour
    {
        public System.Action OnAnimationFinished;

        public float TotalTime = 1;
        public float Distance = 5;
        public Vector3 VectorDir = new Vector3(0, 1, 0);
        public AnimationCurve CurvePing;
        public AnimationCurve CurvePong;

        private float m_CurTime;
        private bool m_IsActive = false;
        private bool m_UpMovement = true;
        private Vector3 m_InitPos;
        private Vector3 m_From;
        private Vector3 m_To;
        private AnimationCurve m_Curve;

        public void Play()
        {
            if (m_IsActive)
                return;

            m_From = transform.position;

            if (m_UpMovement)
            {
                m_InitPos = transform.position;
                m_To = m_InitPos + VectorDir * Distance;
                m_Curve = CurvePing;
            }
            else
            {
                m_To = m_InitPos;
                m_Curve = CurvePong;
            }

            m_CurTime = 0;
            m_IsActive = true;
        }

        void Update()
        {
            if (m_IsActive)
            {
                m_CurTime += Time.deltaTime;
                transform.position = Vector3.Slerp(m_From, m_To, m_Curve.Evaluate(m_CurTime / TotalTime));

                if (m_CurTime >= TotalTime)
                {
                    m_IsActive = false;
                    transform.position = m_To;

                    if (m_UpMovement)
                    {
                        m_UpMovement = false;
                        Play();
                    }
                    else
                    {
                        m_UpMovement = true;

                        if (OnAnimationFinished != null)
                            OnAnimationFinished();
                    }
                }
            }
        }
    }
}
