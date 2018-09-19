using mytest.Main.MiniGames.ChargeGenerator.UI;
using UnityEngine;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public class Item_Charger : Item<UI_Charger>
    {
        private static int m_Index = 0;

        [Header("Settings")]
        public Data[] ChargeData;
        public float ResetSpeed = 100;
        public ParticleSystem Effect;

        private float m_Speed = 10;
        private float m_CurrentCharge = 0;

        private const float m_MAX_CHARGE = 100;
        
        public void Init()
        {
            ItemUI.Init(ChargeData[m_Index].MinPowerForCharge, ChargeData[m_Index].MaxPowerForCharge);

            UpdateEffect();
        }

        public void AddCharge()
        {
            m_CurrentCharge = Mathf.Clamp(m_CurrentCharge + ChargeData[m_Index].ChargePerAdd, 0, m_MAX_CHARGE);
            UpdateEffect();
        }

        public bool CanSpark()
        {
            bool result = m_CurrentCharge >= ChargeData[m_Index].MinPowerForCharge && m_CurrentCharge <= ChargeData[m_Index].MaxPowerForCharge;

            if (result)
            {
                m_Speed = ResetSpeed;
                m_Index++;

                if (m_Index == ChargeData.Length)
                    m_Index = ChargeData.Length - 1;
            }

            return result;
        }

        public override void UpdateItem(float deltaTime)
        {
            //Текущее значение заряда всегда стремиться к 0
            m_CurrentCharge = Mathf.MoveTowards(m_CurrentCharge, 0, Time.deltaTime * m_Speed);

            UpdateEffect();

            //Если заряд упал почти до 0 и скорость была не стандартной - задать стандартную скорость (Событие, когда получили искру и весь заряд обнуляеться)
            if (m_CurrentCharge <= 0.1f && m_Speed > ChargeData[m_Index].NormalSpeed)
                m_Speed = ChargeData[m_Index].NormalSpeed;

            //Процент заполнения бара зарядки
            float progress = m_CurrentCharge / m_MAX_CHARGE;
            ItemUI.SetProgress(progress);
        }


        void UpdateEffect()
        {
            int particlesCount = Mathf.RoundToInt(m_CurrentCharge / 10f);
            if (particlesCount < 1)
                Effect.gameObject.SetActive(false);
            else
            {
                if (!Effect.gameObject.activeSelf)
                    Effect.gameObject.SetActive(true);

                ParticleSystem.MainModule main = Effect.main;
                main.maxParticles = particlesCount;

                main.startColor = m_CurrentCharge >= ChargeData[m_Index].MaxPowerForCharge ? Color.red : Color.white;
            } 
        }

        [System.Serializable]
        public struct Data
        {
            public float MinPowerForCharge;
            public float MaxPowerForCharge;
            public float ChargePerAdd;
            public float NormalSpeed;
        }
    }
}
