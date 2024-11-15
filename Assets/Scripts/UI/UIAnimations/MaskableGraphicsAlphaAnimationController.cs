﻿using UnityEngine;
using UnityEngine.UI;

namespace FrameworkPackage.UI.Animations
{
    /// <summary>
    /// Контролирует анимацию изменения прозрачности для MaskableGraphic(картинка, текст)
    /// </summary>
    public class MaskableGraphicsAlphaAnimationController : UIAnimationController_OneValue
    {
        [Header("Link")]
        public MaskableGraphic Element;

        protected override void SetTargetPosition()
        {
            m_TargetPosition = Element.color.a;
        }

        protected override void ApplyPosition(float position)
        {
            Color color = new Color(Element.color.r, Element.color.g, Element.color.b, position);
            Element.color = color;
        }
    }
}
