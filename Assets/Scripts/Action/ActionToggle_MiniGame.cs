using mytest.Interaction;
using mytest.Main.MiniGames;
using UnityEngine;

namespace mytest.ActionTrigger
{
    /// <summary>
    /// Класс тумблера который активирует мини игру
    /// </summary>
    public class ActionToggle_MiniGame : ActionToggle_Base
    {
        [Space(10)]
        public MiniGame_Base GameController;

        public override void InteractByTap()
        {
            base.InteractByTap();

            StartMiniGame();
        }

        void StartMiniGame()
        {
            GameManager.Instance.MiniGameController.OnGameStarted += StartedHandler;
            GameManager.Instance.MiniGameController.OnGameFinished += FinishedHandler;
            GameManager.Instance.MiniGameController.StartGame(GameController);
        }

        void  StartedHandler()
        {
            GameManager.Instance.MiniGameController.OnGameStarted -= StartedHandler;
        }

        void FinishedHandler()
        {
            GameManager.Instance.MiniGameController.OnGameFinished -= FinishedHandler;

            InteractionFinishedHandler();
        }
    }
}
