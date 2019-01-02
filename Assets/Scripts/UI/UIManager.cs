using FrameworkPackage.UI.Animations;
using FrameworkPackage.UI.Windows;
using mytest.UI.InputSystem;
using UnityEngine;
using UnityEngine.UI;

namespace mytest.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        public Button Button_Assist;
        public Button Button_Weapon;
        [Header("Animation Controllers")]
        public UIAnimationController_Base JoystickAnimationController;
        public UIAnimationController_Base AssistantButtonAnimationController;
        public UIAnimationController_Base WeaponButtonAnimationController;
        public UIAnimationController_Base CompassAnimationController;
        public UIAnimationController_Base JumpButtonAnimationController;
        [Header("Animation Controllers Init settings")]
        public bool ShowAssistantButton = true;
        public bool ShowWeaponButton = true;
        public bool ShowCompass = true;
        public bool ShowJumpButton = true;

        private UIWindowsManager m_WindowsManager;

        public UIWindowsManager WindowManager
        {
            get { return m_WindowsManager; }
        }

        void Start()
        {
            m_WindowsManager = GetComponent<UIWindowsManager>();

            //Button
            if (Button_Assist != null)
                Button_Assist.onClick.AddListener(Assist_PressHandler);

            if (Button_Weapon != null)
                Button_Weapon.onClick.AddListener(Weapon_PressHandler);

            //Animation Controller
            if (ShowAssistantButton && AssistantButtonAnimationController != null)
                InputManager.Instance.OnInputStateChange += AssistantButtonAnimationController.PlayAnimation;

            if (ShowWeaponButton && WeaponButtonAnimationController != null)
                InputManager.Instance.OnInputStateChange += WeaponButtonAnimationController.PlayAnimation;

            if (ShowCompass && CompassAnimationController != null)
                InputManager.Instance.OnInputStateChange += CompassAnimationController.PlayAnimation;

#if UNITY_EDITOR
            if (InputManager.Instance.PreferVirtualJoystickInEditor)
                SubscribeForVirtualJoystick();
#else
            SubscribeForVirtualJoystick();
#endif
        }

        public UIWindow_Base ShowWindow(UIWindow_Base source)
        {
            return m_WindowsManager.ShowWindow(source);
        }


        void Assist_PressHandler()
        {
            GameManager.Instance.GameState.Player.UseAssistant();
        }

        void Weapon_PressHandler()
        {
            GameManager.Instance.GameState.Player.UseWeapon();
        }


        void SubscribeForVirtualJoystick()
        {
            InputManager.Instance.OnInputStateChange += JoystickAnimationController.PlayAnimation;

            if (ShowJumpButton && JumpButtonAnimationController != null)
                InputManager.Instance.OnInputStateChange += JumpButtonAnimationController.PlayAnimation;
        }
    }
}
