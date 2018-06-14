using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimationController : UIAnimationController 
{
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
        m_RectTransform.anchoredPosition = new Vector2(position, m_RectTransform.anchoredPosition.y);
    }
}
