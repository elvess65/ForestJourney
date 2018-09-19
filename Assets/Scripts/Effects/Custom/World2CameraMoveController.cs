using mytest.Utils;
using UnityEngine;

namespace mytest.Effects.Custom
{
    public class World2CameraMoveHehaviour : MonoBehaviour
    {
        private bool m_IsActive = false;
        private int m_CountOfCompleted = 0;
        private System.Action m_OnMoveComplete;
        private MoveData m_MoveData;
        private InterpolationData<Vector3>[] LerpData;

        public static World2CameraMoveHehaviour CreateWorld2CameraMoveHehaviour()
        {
            return GameManager.Instance.gameObject.AddComponent<World2CameraMoveHehaviour>();
        }
        
        public void Move(MoveData mData)
        {
            m_OnMoveComplete = mData.OnMoveComplete;
            m_MoveData = mData;

            LerpData = new InterpolationData<Vector3>[m_MoveData.ObjectsToMove.Length];
            for (int i = 0; i < LerpData.Length; i++)
            {
                LerpData[i].From = m_MoveData.ObjectsToMove[i].transform.position;
                LerpData[i].To = Viewport2World(m_MoveData.DestinationViewport[i]);
                LerpData[i].TotalTime = m_MoveData.TotalTime;
                LerpData[i].Start();
            }

            m_IsActive = true;
        }

        public void MoveReserve(System.Action onMoveComplete)
        {
            m_OnMoveComplete = onMoveComplete;

            for (int i = 0; i < LerpData.Length; i++)
            {
                Vector3 from = LerpData[i].From;
                LerpData[i].From = m_MoveData.ObjectsToMove[i].transform.position;
                LerpData[i].To = from;
                LerpData[i].Start();
            }

            m_IsActive = true;
        }


        void Update()
        {
            if (m_IsActive)
            {
                for (int i = 0; i < LerpData.Length; i++)
                {
                    if (LerpData[i].IsStarted)
                    {
                        LerpData[i].Increment();
                        m_MoveData.ObjectsToMove[i].transform.position = Vector3.Lerp(LerpData[i].From, LerpData[i].To, LerpData[i].Progress);

                        if (LerpData[i].Overtime())
                        {
                            LerpData[i].Stop();
                            m_CountOfCompleted++;
                        }
                    }
                }

                if (m_CountOfCompleted == LerpData.Length)
                {
                    m_IsActive = false;
                    m_CountOfCompleted = 0;

                    if (m_OnMoveComplete != null)
                        m_OnMoveComplete();
                }
            }
        }

        public static Vector3 Viewport2World(Vector3 viewPort)
        {
            return Camera.main.ViewportToWorldPoint(viewPort);
        }

        public class MoveData
        {
            public System.Action OnMoveComplete;
            public float TotalTime;

            public Transform[] ObjectsToMove;
            public Vector3[] DestinationViewport;

            public MoveData(System.Action onMoveComplete, float totalTime, Transform[] objectsToMove, Vector3[] destinationViewport)
            {
                OnMoveComplete = onMoveComplete;
                TotalTime = totalTime;
                ObjectsToMove = objectsToMove;
                DestinationViewport = destinationViewport;
            }
        }
    }
}
