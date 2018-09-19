using UnityEngine;
using UnityEngine.UI;

namespace mytest.Main.MiniGames.ChargeGenerator.UI
{
    public class UI_Charger : UI_Base_AnimationShow
    {
        [Header("Images")]
        public Image Image_PowerState;
        public Image Image_ChargeArea;

        public void Init(float minPowerForCharge, float maxPowerForCharge)
        {
            //Выставить положение зоны зарядки

            //Высота спрайта бара мощности (в пикселях)
            float powerBarRelativeHeight = Image_PowerState.rectTransform.sizeDelta.y;
            //Размер зоны (в процентах), где доступна зарядка
            float chargeZoneSize = maxPowerForCharge - minPowerForCharge;
            //Высота спрайта зоны зарядки (в пикселях)
            float chargeAreaRelativeHeight = powerBarRelativeHeight * (chargeZoneSize / 100f);
            //Задать высоту спрайта зоны зарядки
            Image_ChargeArea.rectTransform.sizeDelta = new Vector2(Image_ChargeArea.rectTransform.sizeDelta.x, chargeAreaRelativeHeight);

            //Координата по У где должен располагаться спрайт зоны зарядки
            float pos = powerBarRelativeHeight * (minPowerForCharge / 100);
            //Задать координату по высоте спрайту зоны зарядки
            Image_ChargeArea.rectTransform.anchoredPosition = new Vector2(Image_ChargeArea.rectTransform.anchoredPosition.x, pos);
        }

        public void SetProgress(float progress)
        {
            Image_PowerState.fillAmount = progress;
        }
    }
}
