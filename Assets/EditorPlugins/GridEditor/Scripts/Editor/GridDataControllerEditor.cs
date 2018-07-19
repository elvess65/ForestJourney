using UnityEditor;
using UnityEngine;

namespace GridEditor
{
	[CustomEditor(typeof(GridDataController))]
    public class GridDataControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

			GridDataController source = (GridDataController)target;
            GridController gridController = source.GetComponent<GridController>();

			if (source.DataExists && GUILayout.Button("Serialize"))
			{
				source.SerializeData();
			}

			if (GUILayout.Button("Deserialize"))
			{
				source.DeserializeData();
			}

            if (source.DataExists && GUILayout.Button("Clear local data"))
            {
                source.Clear();
            }
        }
    }
}
