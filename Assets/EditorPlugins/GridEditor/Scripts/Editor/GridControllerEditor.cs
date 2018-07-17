﻿using UnityEditor;
using UnityEngine;

namespace GridEditor
{
    [CustomEditor(typeof(GridController))]
    public class GridControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GridController source = (GridController)target;
            GridDataController dataController = source.GetComponent<GridDataController>();

            if (GUILayout.Button("Create Default Grid"))
            {
                source.Clear();
                source.CreateDefaultGrid();
            }

            if (dataController.DataExists && GUILayout.Button("Create Grid From Data"))
            {
                source.Clear();
                source.LoadGridData();
            }

            if (source.GridExists && GUILayout.Button("Save Data"))
            {
                source.SaveGridData();
            }

            if (source.GridExists && GUILayout.Button("Clear"))
            {
                source.Clear();
            }
        }
    }
}
