using UnityEngine;

namespace mytest.Effects.Custom
{
    /// <summary>
    /// Подпрыгивание объекта
    /// </summary>
    public class ScriptEffect_JumpObject : MonoBehaviour
    {
        public System.Action OnAnimationFinished;

        public float Gravity = 9.8f;
        public float VerticalVelocity = 10f;

        private bool m_IsActive = false;
        private float m_CurVerticalVelocity;
        private float m_VerticalInitPosition; 

        public void Play()
        {
            if (m_IsActive)
                return;

            m_VerticalInitPosition = transform.position.y;
            m_CurVerticalVelocity = VerticalVelocity;
            m_IsActive = true;
        }

        void Update()
        {
            if (m_IsActive)
            {
                Vector3 pos = transform.position;
                pos.y += m_CurVerticalVelocity * Time.deltaTime;
                transform.position = pos;

                AffectGravity();

                if (transform.position.y < m_VerticalInitPosition)
                {
                    m_IsActive = false;

                    pos = transform.position;
                    pos.y = m_VerticalInitPosition;
                    transform.position = pos;

                    if (OnAnimationFinished != null)
                        OnAnimationFinished();
                }
            }
        }

        void AffectGravity()
        {
            m_CurVerticalVelocity -= Gravity * Time.deltaTime;
        }
    }
}
