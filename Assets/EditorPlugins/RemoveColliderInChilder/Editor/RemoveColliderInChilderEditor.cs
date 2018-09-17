using UnityEditor;
using UnityEngine;

namespace mytest.EditorPlugins.Tools.RemoveColliderInChildren
{
    [CustomEditor(typeof(RemoveColliderInChilder))]
    public class RemoveColliderInChilderEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Disable colliders"))
            {
                RemoveColliderInChilder controller = (RemoveColliderInChilder)target;
                controller.DisableColliders();

                DestroyImmediate(this);
            }
        }   
    }
}
