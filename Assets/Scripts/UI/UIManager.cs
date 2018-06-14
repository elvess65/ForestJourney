using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button Button_Assist;
    public Button Button_Weapon;
    [Header("Animation Controllers")]
    public UIAnimationController JoystickAnimationController;
    public UIAnimationController AssistantButtonAnimationController;
    public UIAnimationController WeaponButtonAnimationController;
    [Header("Animation Controllers Init settings")]
    public bool ShowAssistantButton = true;
    public bool ShowWeaponButton = true;

	void Start ()
    {
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
	}


    public void Assist_PressHandler()
    {
        GameManager.Instance.GameState.Player.UseAssistant();
    }

    public void Weapon_PressHandler()
    {
        GameManager.Instance.GameState.Player.UseWeapon();
    }
}
