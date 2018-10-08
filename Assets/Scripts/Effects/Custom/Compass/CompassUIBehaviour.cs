using UnityEngine;

namespace mytest.Effects.Custom.Compass
{
    public class CompassUIBehaviour : MonoBehaviour
    {
        public Transform Graphics;

        private float m_PrevRotation = 0;

        void LateUpdate()
        {
            Graphics.transform.localRotation = Quaternion.Euler(0, 0, GameManager.Instance.CameraController.transform.localEulerAngles.y);
        }
    }
}
