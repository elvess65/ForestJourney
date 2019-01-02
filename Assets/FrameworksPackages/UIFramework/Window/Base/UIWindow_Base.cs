using FrameworkPackage.UI.Animations;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Windows
{
    /// <summary>
    /// Базовое окно, которое умеет показываться и прятаться
    /// </summary>
    public abstract class UIWindow_Base : UIObject
    {
        /// <summary>
        /// Начало закрывания окна (до анимации)
        /// </summary>
        public event System.Action OnUIBeginHiding;
        /// <summary>
        /// Окончание анимации закрывания окна
        /// </summary>
        public event System.Action OnUIHided;

        public Text Text_Main;
        public UIAnimationController_Base[] AnimationControllers;

        public virtual void Show()
        {
            Init();

            if (AnimationControllers.Length > 0)
            {
                AnimationControllers[0].OnShowFinished += ShowAnimation_Finished;
                for (int i = 0; i < AnimationControllers.Length; i++)
                    AnimationControllers[i].PlayAnimation(true);
            }
            else
                ShowAnimation_Finished();
        }

        public virtual void Hide()
        {
            if (OnUIBeginHiding != null)
                OnUIBeginHiding();

            if (AnimationControllers.Length > 0)
            {
                AnimationControllers[0].OnHideFinished += HideAnimation_Finished;
                for (int i = 0; i < AnimationControllers.Length; i++)
                    AnimationControllers[i].PlayAnimation(false);
            }
            else
                HideAnimation_Finished();
        }

        public virtual void HideByEscape()
        {
            Hide();
        }


        protected override void Init()
        {
            base.Init();
        }

        protected virtual void ShowAnimation_Finished()
        { }

        protected virtual void HideAnimation_Finished()
        {
            if (OnUIHided != null)
                OnUIHided();

            Destroy(gameObject);
        }
    }
}
