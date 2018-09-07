using mytest.Effects;
using mytest.Main.MiniGames.ChargeGenerator.UI;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public class Item_Generator : Item<UI_Generator>
    {
        public System.Action OnCharged;

        public int ChargesToFill = 2;
        public Effect_Base[] ChargeEffects;
        public Effect_Base[] ChargedEffects;

        private int m_CurrentCharge = 0;

        public void Init()
        {
            ItemUI.Init();
        }

        public bool AddCharge()
        {
            ChargeEffects[m_CurrentCharge].Activate();

            m_CurrentCharge++;

            ItemUI.SetProgress(m_CurrentCharge / (float)ChargesToFill);
            
            //Еслми генератор заполнен
            if (m_CurrentCharge == ChargesToFill)
            {
                for (int i = 0; i < ChargedEffects.Length; i++)
                    ChargedEffects[i].Activate();

                if (OnCharged != null)
                    OnCharged();

                return true;
            }

            return false;
        }

        public override void UpdateItem(float deltaTime)
        {
            
        }
    }
}
