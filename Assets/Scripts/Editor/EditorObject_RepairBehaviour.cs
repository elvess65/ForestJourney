using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Object_RepairBehaviour))]
public class EditorObject_RepairBehaviour : Editor
{
    private bool m_IsRepaired = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Object_RepairBehaviour source = (Object_RepairBehaviour)target;

        if (GUILayout.Button("Save as Repaired"))
        {
            if (source.ObjectItems == null)
                source.ObjectItems = new System.Collections.Generic.List<Object_RepairBehaviour_Item>();
            else
                source.ObjectItems.Clear();


            MeshRenderer[] renderers = source.transform.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                Object_RepairBehaviour_Item item = renderers[i].gameObject.GetComponent<Object_RepairBehaviour_Item>();
                if (item == null)
                    item = renderers[i].gameObject.AddComponent<Object_RepairBehaviour_Item>();
                
                item.SaveTransformAsRepaired();

                source.ObjectItems.Add(item);
            }
        }

        if (source.ObjectItems != null && source.ObjectItems.Count > 0)
        {
            if (GUILayout.Button("Save as Destroyed"))
            {
                foreach (Object_RepairBehaviour_Item item in source.ObjectItems)
                    item.SaveTransfromAsDestroyed();
            }

            if (!m_IsRepaired && GUILayout.Button("Show as Repaired"))
            {
                m_IsRepaired = true;
                source.SetRepairedImmediate();
            }

            if (m_IsRepaired && GUILayout.Button("Show as Destroyed"))
            {
                m_IsRepaired = false;
                source.SetDestroyedImmediate();
            }
        }
    }
}
