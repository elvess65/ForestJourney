using FrameworkPackage.UI.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace mytest.UI.Animations
{
    /// <summary>
    /// Контролирует анимацию расположения кнопок
    /// </summary>
    public class ButtonAnimationController : UIAnimationController_OneValue
    {
        [Header("Link")]
        public Button UIButton;

        private RectTransform m_RectTransform;

        protected override void SetTargetPosition()
        {
            //Получить текущую позицию кнопки
            m_RectTransform = UIButton.transform as RectTransform;

            m_TargetPosition = m_RectTransform.anchoredPosition.x;
        }

        protected override void ApplyPosition(float position)
        {
            //Задать позицию кнопки
            m_RectTransform.anchoredPosition = new Vector2(position, m_RectTransform.anchoredPosition.y);
        }
    }
}
