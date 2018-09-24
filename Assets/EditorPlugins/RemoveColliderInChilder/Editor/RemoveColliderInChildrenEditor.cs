using UnityEditor;
using UnityEngine;

namespace mytest.EditorPlugins.Tools.RemoveColliderInChildren
{
    [CustomEditor(typeof(RemoveColliderInChildren))]
    public class RemoveColliderInChildrenEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Disable colliders"))
            {
                RemoveColliderInChildren controller = (RemoveColliderInChildren)target;
                controller.DisableColliders();
            }
        }   
    }
}
