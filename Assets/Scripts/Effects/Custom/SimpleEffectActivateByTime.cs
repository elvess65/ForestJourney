using UnityEngine;

namespace mytest.Effects.Custom
{
    public class SimpleEffectActivateByTime : MonoBehaviour
    {
        public MinMaxRangeSlider.MinMaxPair TimeRange = new MinMaxRangeSlider.MinMaxPair(0.1f, 10f);

        
        void Start()
        {
            float randomDelay = Random.Range(TimeRange.Min, TimeRange.Max);                
        }

        void Update()
        {

        }
    }
}
