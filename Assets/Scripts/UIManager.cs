using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button Button_Assist;
    public Button Button_Weapon;

	void Start ()
    {
        Button_Assist.onClick.AddListener(Assist_PressHandler);
        Button_Weapon.onClick.AddListener(Weapon_PressHandler);
    }

    public void Assist_PressHandler()
    {
        GameManager.Instance.Player.UseAssistant();
    }

    public void Weapon_PressHandler()
    {
        GameManager.Instance.Player.UseWeapon();
    }
}
