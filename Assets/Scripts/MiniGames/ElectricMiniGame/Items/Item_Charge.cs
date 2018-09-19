using UnityEngine;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public class Item_Charge : BaseItem
    {
        private bool m_IsPressed = false;
        private Camera m_Cam;
        private Collider m_Collider;
        private Vector3 m_Offset;
        private Vector3 m_ScreenPoint;

        public void Init(Camera cam)
        {
            m_Cam = cam;
            m_Collider = GetComponent<Collider>();
        }

        public void HandleMouseDown()
        {
            m_IsPressed = true;

            m_Collider.enabled = false;
            m_ScreenPoint = m_Cam.WorldToScreenPoint(transform.position);
            m_Offset = transform.position - m_Cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z));
        }

        public void HandleMouseUp()
        {
            m_Collider.enabled = true;
            m_IsPressed = false;
        }

      
        public override void UpdateItem(float deltaTime)
        {
            if (!m_IsPressed)
                return;

            Vector3 cursorScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_ScreenPoint.z);
            Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorScreenPoint) + m_Offset;
            cursorPosition.z = transform.position.z;

            transform.position = cursorPosition;
        }
    }
}
