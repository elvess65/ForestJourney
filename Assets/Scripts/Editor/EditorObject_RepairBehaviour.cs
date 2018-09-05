using mytest.Effects.Custom.RepairObject;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RepairObjectBehaviour))]
public class EditorObject_RepairBehaviour : Editor
{
    private bool m_IsRepaired = true;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RepairObjectBehaviour source = (RepairObjectBehaviour)target;

        if (GUILayout.Button("Save as Repaired"))
        {
            if (source.ObjectItems == null)
                source.ObjectItems = new System.Collections.Generic.List<RepairObjectBehaviour_Item>();
            else
                source.ObjectItems.Clear();


            MeshRenderer[] renderers = source.transform.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renderers.Length; i++)
            {
                RepairObjectBehaviour_Item item = renderers[i].gameObject.GetComponent<RepairObjectBehaviour_Item>();
                if (item == null)
                    item = renderers[i].gameObject.AddComponent<RepairObjectBehaviour_Item>();
                
                item.SaveTransformAsRepaired();

                source.ObjectItems.Add(item);
            }
        }

        if (source.ObjectItems != null && source.ObjectItems.Count > 0)
        {
            if (GUILayout.Button("Save as Destroyed"))
            {
                foreach (RepairObjectBehaviour_Item item in source.ObjectItems)
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

			if (GUILayout.Button("Clear"))
			{
				foreach (RepairObjectBehaviour_Item item in source.ObjectItems)
					DestroyImmediate (item);

				source.ObjectItems.Clear ();
				m_IsRepaired = true;
			}
        }
    }
}
