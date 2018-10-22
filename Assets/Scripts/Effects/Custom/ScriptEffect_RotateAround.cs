using MinMaxRangeSlider;
using UnityEngine;

namespace mytest.Effects.Custom
{
    /// <summary>
    /// Вращение вокруг оси с определенной скоростью
    /// </summary>
    public class ScriptEffect_RotateAround : MonoBehaviour
    {
        public float RotationSpeed = 20;
        public Vector3 Axis = Vector3.forward;
        [Header("Random Axis")]
        public bool RandomRotation = false;
        public bool RandomSpeed = true;
        public float MinSpeed = 20;
        public float MaxSpeed = 100;

        private void Start()
        {
            if (RandomRotation)
                Axis = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));

            if (RandomSpeed)
                RotationSpeed = Random.Range(MinSpeed, MaxSpeed);
        }

        void Update()
        {
            transform.Rotate(Axis, Time.deltaTime * RotationSpeed);
        }
    }
}
