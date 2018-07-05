﻿using UnityEngine;
using UnityEngine.UI;

public class TriggerEvent_ShowButtonAndHightlight : TriggerAction_Event 
{
    public RectTransform ButtonHightlight;
    public Button Target;
    public UIAnimationController AnimationController;

    private RectTransform m_ButtonHightlght;

    public override void StartEvent()
    {
        //Показать кнопку и добавить событие на нажатие
        AnimationController.PlayAnimation(true);
        Target.onClick.AddListener(ButtonAssist_PressHandler);

        //Создать выделение
        m_ButtonHightlght = Instantiate(ButtonHightlight);
        m_ButtonHightlght.SetParent(Target.transform, false);

        //Показать затемнение
        GameManager.Instance.UIManager.WindowManager.ShowScreenFade();
    }

    void ButtonAssist_PressHandler()
    {
        //Отписатья от события и удалить выделение
        Target.onClick.RemoveListener(ButtonAssist_PressHandler);
        Destroy(m_ButtonHightlght.gameObject);

        //Спрятать затемнение
        GameManager.Instance.UIManager.WindowManager.HideScreenFade();

        //Включить ввод и подписаться на обновление кнопки 
        InputManager.Instance.InputIsEnabled = true;
        InputManager.Instance.OnInputStateChange += AnimationController.PlayAnimation;
    }
}