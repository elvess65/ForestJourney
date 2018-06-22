using UnityEngine;
using UnityEngine.UI;

public class TriggerEvent_ShowButtonAndHightlight : TriggerAction_Event 
{
    public RectTransform ButtonHightlight;
    public Button Target;
    public UIAnimationController AnimationController;

    private RectTransform m_ButtonHightlght;

    public override void StartEvent()
    {
        AnimationController.PlayAnimation(true);
        Target.onClick.AddListener(ButtonAssist_PressHandler);

        m_ButtonHightlght = Instantiate(ButtonHightlight);
        m_ButtonHightlght.SetParent(Target.transform, false);

        GameManager.Instance.UIManager.WindowManager.ShowScreenFade();
    }

    void ButtonAssist_PressHandler()
    {
        Target.onClick.RemoveListener(ButtonAssist_PressHandler);
        Destroy(m_ButtonHightlght.gameObject);


        GameManager.Instance.UIManager.WindowManager.HideScreenFade();
        InputManager.Instance.InputIsEnabled = true;
    }
}
