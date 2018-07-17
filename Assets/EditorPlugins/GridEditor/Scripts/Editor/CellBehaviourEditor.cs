using UnityEditor;
using UnityEngine;

namespace GridEditor
{
    [CustomEditor(typeof(CellBehaviour))]
    public class CellBehaviourEditor : Editor
    {

        private bool m_WaitingForLinkSelection = false;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            CellBehaviour source = (CellBehaviour)target;

            if (GUILayout.Button("Link"))
            {
                m_WaitingForLinkSelection = true;
            }
        }

        void OnSceneGUI()
        {
            if (m_WaitingForLinkSelection && Event.current.type == EventType.MouseDown)
            {
                m_WaitingForLinkSelection = false;

                RaycastHit hit;
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    CellBehaviour linkedCell = hit.collider.GetComponent<CellBehaviour>();

                    if (linkedCell != null)
                    {
                        CellBehaviour curCell = (CellBehaviour)target;
                        curCell.LinkCell(linkedCell);
                        linkedCell.LinkCell(curCell);
                    }
                }
            }
        }
    }
}
