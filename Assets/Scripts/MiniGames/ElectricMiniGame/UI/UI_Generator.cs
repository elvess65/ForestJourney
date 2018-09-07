using UnityEngine;
using UnityEngine.UI;

namespace mytest.Main.MiniGames.ChargeGenerator.UI
{
    public class UI_Generator : UI_Base
    {
        [Header("Images")]
        public Image Image_ChargeState;

        public void Init()
        {
            //Обнулить прогресс генератора
            SetProgress(0);
        }

        public void SetProgress(float progress)
        {
            Image_ChargeState.fillAmount = progress;
        }
    }
}
