using UnityEngine;

namespace GridEditor
{
    public class GridDataController : MonoBehaviour
    {
        private CellData[,] m_Data;

        public bool DataExists
        {
            get { return m_Data != null && m_Data.Length > 0; }
        }

        public void SaveData(CellBehaviour[,] curArr, int gridSize)
        {
            m_Data = new CellData[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    m_Data[i, j] = curArr[i, j].GetCellData();
                }
            }
        }

        public CellData[,] GetData()
        {
            return m_Data;
        }
    }
}
