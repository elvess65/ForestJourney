using mytest.Effects.PostProcessing;
using mytest.UI.Animations;
using mytest.UI.InputSystem;
using mytest.UI.Loading;
using UnityEngine;

namespace mytest.Main
{
    public class GameOverController : MonoBehaviour
    {
        public GameObject Parent;
        [Header("Animation Controllers")]
        public BaseUIAnimationController UIAnimationController_Image_BG;
        public BaseUIAnimationController UIAnimationController_Text_Tap;
        public BaseUIAnimationController UIAnimationController_Text_GameOver;
        
        private bool m_AnimationFinished = false;

        void Start()
        {
            UIAnimationController_Image_BG.OnShowFinished += UIAnimationController_Text_Tap_OnShowFinished;

            if (Parent.activeSelf)
                Parent.SetActive(false);
        }

        void Update()
        {
            if (m_AnimationFinished)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    m_AnimationFinished = false;

                    UIAnimationController_Text_Tap.PingPong = false;
                    UIAnimationController_Text_GameOver.PlayAnimation(false);
                    UIAnimationController_Text_Tap.PlayAnimation(false);
                    UIAnimationController_Image_BG.PlayAnimation(false);

                    LevelLoader.Instance.RestartRound();
                }
            }
        }

        public void GameOver()
        {
            Parent.SetActive(true);

            InputManager.Instance.InputIsEnabled = false;
            GameManager.Instance.CameraController.enabled = false;
            PostProcessingController.Instance.DecreaseSaturation();

            UIAnimationController_Text_GameOver.PlayAnimation(true);
            UIAnimationController_Text_Tap.PlayAnimation(true);
            UIAnimationController_Image_BG.PlayAnimation(true);
        }

        private void UIAnimationController_Text_Tap_OnShowFinished()
        {
            UIAnimationController_Image_BG.OnShowFinished -= UIAnimationController_Text_Tap_OnShowFinished;

            m_AnimationFinished = true;
        } 
    }
}
