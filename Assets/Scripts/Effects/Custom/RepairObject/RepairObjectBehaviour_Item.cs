using UnityEngine;

namespace mytest.Effects.Custom.RepairObject
{
    /// <summary>
    /// Элемент объекта, который может быть разобраным или сораным
    /// </summary>
    public class RepairObjectBehaviour_Item : MonoBehaviour
    {
        public System.Action<int> OnAllowAnimateNext;

        [Header("Params")]
        public TransformData RepairedTransfromData;
        public TransformData DestroyedTransformData;
        public int GroupID = 0;
        [Header("Animation")]
        [MinMaxRangeSlider.MinMax(0, 1)]
        public MinMaxRangeSlider.MinMaxPair ProgressToNextItem = new MinMaxRangeSlider.MinMaxPair(0.2f, 0.5f);
        public AnimationCurve CurvePosition = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(1, 1) });

        private Collider m_Collider;
        private TransformData m_From;
        private TransformData m_To;
        private float m_ProgressToNextItem;
        private Vector3 m_AnimationPositionDistance;
        private System.Action OnAnimationFinished;

        private float m_CurTime;
        private float m_TotalTime = 0.5f;
        private bool m_IsAnimating = false;
        private bool m_IsRepaired = true;

        public float AnimationTime
        {
            set { m_TotalTime = value; }
        }

        public void SetRepairedImmediate()
        {
            m_IsRepaired = true;
            transform.localPosition = RepairedTransfromData.Position;
            transform.localRotation = RepairedTransfromData.Rotation;
        }

        public void SetDestroyedImmediate()
        {
            m_IsRepaired = false;
            transform.localPosition = DestroyedTransformData.Position;
            transform.localRotation = DestroyedTransformData.Rotation;
        }

        //Editor
        public void SaveTransformAsRepaired()
        {
            RepairedTransfromData = GetTransform();
        }

        public void SaveTransfromAsDestroyed()
        {
            DestroyedTransformData = GetTransform();
        }

        //Animation
        public void Animate()
        {
            if (m_IsRepaired)
            {
                if (m_Collider != null)
                    m_Collider.enabled = false;

                transform.Translate(Vector3.forward, Space.World);
                OnAnimationFinished = SetDestroyedImmediate;
            }
            else
            {
                if (m_Collider != null)
                    m_Collider.enabled = true;

                OnAnimationFinished = SetRepairedImmediate;
            }

            m_To = m_IsRepaired ? DestroyedTransformData : RepairedTransfromData;

            m_IsRepaired = !m_IsRepaired;

            m_From = GetTransform();
            m_ProgressToNextItem = Random.Range(ProgressToNextItem.Min, ProgressToNextItem.Max);
            m_AnimationPositionDistance = m_To.GetDistance(m_From);

            m_CurTime = 0;
            m_IsAnimating = true;
        }

        void Start()
        {
            m_Collider = GetComponent<Collider>();
        }

        void Update()
        {
            if (m_IsAnimating)
            {
                m_CurTime += Time.deltaTime;
                float progress = m_CurTime / m_TotalTime;

                if (progress >= m_ProgressToNextItem && OnAllowAnimateNext != null)
                {
                    OnAllowAnimateNext(GroupID);
                    OnAllowAnimateNext = null;
                }

                UpdateTransform(progress);

                if (progress >= 1)
                {
                    m_IsAnimating = false;

                    if (OnAnimationFinished != null)
                        OnAnimationFinished();
                }
            }
        }

        void UpdateTransform(float progress)
        {
            transform.localPosition = m_From.Position + CurvePosition.Evaluate(progress) * m_AnimationPositionDistance;
            transform.localRotation = Quaternion.Slerp(m_From.Rotation, m_To.Rotation, progress);
        }

        TransformData GetTransform()
        {
            return new TransformData(transform.localPosition, transform.localRotation);
        }

        [System.Serializable]
        public struct TransformData
        {
            public Vector3 Position;
            public Quaternion Rotation;

            public TransformData(Vector3 pos, Quaternion rot)
            {
                Position = pos;
                Rotation = rot;
            }

            public Vector3 GetDistance(TransformData data)
            {
                return Position - data.Position;
            }
        }
    }
}
