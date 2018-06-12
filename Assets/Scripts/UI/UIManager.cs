using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button Button_Assist;
    public Button Button_Weapon;

    private JoystickAnimationController m_JoystickAnimationController;

	void Start ()
    {
        m_JoystickAnimationController = GetComponent<JoystickAnimationController>();
        if (m_JoystickAnimationController != null)
            InputManager.Instance.OnInputStateChange += m_JoystickAnimationController.PlayAnimation;

        if (Button_Assist != null)
            Button_Assist.onClick.AddListener(Assist_PressHandler);

        if (Button_Weapon != null)
            Button_Weapon.onClick.AddListener(Weapon_PressHandler);
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
