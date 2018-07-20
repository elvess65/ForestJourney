using UnityEngine;

namespace GridEditor
{
    public class CellConnection : MonoBehaviour
    {
        public GridTools.VerticalDirections VerticalDirToLinkedCell;
        [Tooltip("Higher, Lower, Same")]
        public GameObject[] VerticalDirectionObjects;

        public void SetVerticaDirection(GridTools.VerticalDirections vDir)
        {
            VerticalDirToLinkedCell = vDir;

            UpdateDirectionSign(vDir);
        }

        void UpdateDirectionSign(GridTools.VerticalDirections vDir)
        {
			for (int i = 0; i < VerticalDirectionObjects.Length; i++)
				VerticalDirectionObjects[i].SetActive(false);

            if (vDir != GridTools.VerticalDirections.Same)
                VerticalDirectionObjects[(int)vDir].SetActive(true);
        }
    }
}
