using UnityEngine;

namespace mytest.Main.MiniGames
{
    public class MiniGameController : MonoBehaviour
    {
        public System.Action OnGameStarted;
        public System.Action OnGameFinished;

        public Camera MiniGameCamera;
        public LayerMask Layer;

        private MiniGame_Base m_MiniGame;
        private bool m_ProcessGame = false;

        public void StartGame(MiniGame_Base game)
        {
            MiniGameCamera.gameObject.SetActive(true);

            m_MiniGame = game;
            m_MiniGame.OnGameStarted += GameStartedHandler;
            m_MiniGame.OnGameFinished += GameFinishedHandler;

            m_MiniGame.StartGame(MiniGameCamera, Layer);
        }

        void GameStartedHandler()
        {
            if (OnGameStarted != null)
                OnGameStarted();

            m_ProcessGame = true;
        }

        void GameFinishedHandler()
        {
            if (OnGameFinished != null)
                OnGameFinished();
        }

        private void Update()
        {
            if (m_ProcessGame && m_MiniGame != null)
                m_MiniGame.ProcessGame(Time.deltaTime);
        }
    }

    public abstract class MiniGame_Base : MonoBehaviour
    {
        public System.Action OnGameStarted;
        public System.Action OnGameFinished;

        protected Camera m_Cam;
        protected LayerMask m_Layer;

        public virtual void StartGame(Camera cam, LayerMask layer)
        {
            m_Cam = cam;
            m_Layer = layer;

            if (OnGameStarted != null)
                OnGameStarted();
        }

        public virtual void FinishGame()
        {
            if (OnGameFinished != null)
                OnGameFinished();
        }

        public abstract void ProcessGame(float deltaTime);
    }
}
