using System.Collections.Generic;
using UnityEngine;

namespace GridEditor
{
    public class GridDataController : MonoBehaviour
    {
        private SavedData m_Data;

        public bool DataExists
        {
            get { return m_Data != null && m_Data.CellsData.Count > 0; }
        }

        public void SaveData(CellBehaviour[,] cellsArr, int gridWidth, int gridHeight, int enviromentIndex)
        {
			m_Data = new SavedData(gridWidth, gridHeight, enviromentIndex);
			
            for (int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    m_Data.CellsData.Add(cellsArr[i, j].GetCellData());
                }
            }
        }

        public SavedData GetData()
        {
            return m_Data;
        }

        public void SerializeData()
        {
			Serialization.XMLSerializer.Serialize(m_Data, "data.xml");
        }

        public void DeserializeData()
        {
            m_Data = Serialization.XMLSerializer.Deserialize<GridDataController.SavedData>("data.xml");
        }

        public void Clear()
        {
            m_Data = null;
        }

        public class SavedData
        {
            public int GridWidth;
            public int GridHeight;
            public int EnviromentIndex;
            public List<CellData> CellsData;

            public SavedData()
            {
                GridWidth = GridHeight = EnviromentIndex = 0;
                CellsData = new List<CellData>();
            }

            public SavedData(int gridWidth, int gridHeight, int enviromentIndex)
            {
                GridWidth = gridWidth;
                GridHeight = gridHeight;
                EnviromentIndex = enviromentIndex;
                CellsData = new List<CellData>();
            }
        }
    }
}
