using mytest.UI.Animations;
using mytest.UI.InputSystem;
using mytest.UI.Windows;
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
        public BaseUIAnimationController JoystickAnimationController;
        public BaseUIAnimationController AssistantButtonAnimationController;
        public BaseUIAnimationController WeaponButtonAnimationController;
        public BaseUIAnimationController CompassAnimationController;
        [Header("Animation Controllers Init settings")]
        public bool ShowAssistantButton = true;
        public bool ShowWeaponButton = true;
        public bool ShowCompass = true;

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
            InputManager.Instance.OnInputStateChange += JoystickAnimationController.PlayAnimation;

            if (ShowAssistantButton && AssistantButtonAnimationController != null)
                InputManager.Instance.OnInputStateChange += AssistantButtonAnimationController.PlayAnimation;

            if (ShowWeaponButton && WeaponButtonAnimationController != null)
                InputManager.Instance.OnInputStateChange += WeaponButtonAnimationController.PlayAnimation;

            if (ShowCompass && CompassAnimationController != null)
                InputManager.Instance.OnInputStateChange += CompassAnimationController.PlayAnimation;
        }

        public void Assist_PressHandler()
        {
            GameManager.Instance.GameState.Player.UseAssistant();
        }

        public void Weapon_PressHandler()
        {
            GameManager.Instance.GameState.Player.UseWeapon();
        }

        public UIWindow_Base ShowWindow(UIWindow_Base source)
        {
            return m_WindowsManager.ShowWindow(source);
        }
    }
}
