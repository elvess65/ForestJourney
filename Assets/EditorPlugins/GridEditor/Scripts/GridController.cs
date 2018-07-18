using UnityEngine;

namespace GridEditor
{
    public class GridController : MonoBehaviour
    {
        public CellBehaviour CellBehaviourPrefab;
        [Header("Settings")]
        public int CellSize = 10;
        public int GridWidth = 3;
        public int GridHeight = 3;
        public int EnviromentIndex = 0;

        private CellBehaviour[,] m_Cells = null;

        public bool GridExists
        {
            get { return m_Cells != null && m_Cells.Length > 0; }
        }

        public void LoadGridData()
        {
            GridDataController dataController = GetComponent<GridDataController>();

            GridDataController.SavedData data = dataController.GetData();
            m_Cells = new CellBehaviour[data.GridWidth, data.GridHeight];

            //Создать ячейки
            for (int i = 0; i < data.CellsData.Count; i++)
            {
                CellData cellData = data.CellsData[i];
                Cell rootCellData = cellData.RootCell;

                CellBehaviour cell = CreateCell(rootCellData.X, rootCellData.Y);
				cell.SetCellData(cellData);
            }

            //Создать связи между ячейками
            for (int i = 0; i < data.GridWidth; i++)
            {
                for (int j = 0; j < data.GridHeight; j++)
                {
                    //Если у ячейки есть связи
                    CellData cellData = m_Cells[i, j].GetCellData();
                    if (cellData.LinkedCells.Count > 0)
                    {
                        for (int index = 0; index < cellData.LinkedCells.Count; index++)
                        {
                            //Координаты связанной ячейки
                            int x = cellData.LinkedCells[index].X;
                            int y = cellData.LinkedCells[index].Y;

                            //Связать ячейки
                            m_Cells[i, j].LinkCell(m_Cells[x, y], false);
                        }
                    }
                }
            }
        }

        public void SaveGridData()
        {
            GridDataController dataController = GetComponent<GridDataController>();
            dataController.SaveData(m_Cells, GridWidth, GridHeight, EnviromentIndex);
        }

        public void CreateDefaultGrid()
        {
            m_Cells = new CellBehaviour[GridWidth, GridHeight];

            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    CellBehaviour cell = CreateCell(i, j);
                    cell.SetCellData(new CellData(new Cell(i, j)));
                }
            }
        }

        public void Clear()
        {
            if (!GridExists)
                return;

            for (int i = 0; i < GridWidth; i++)
            {
                for (int j = 0; j < GridHeight; j++)
                {
                    if (m_Cells[i, j] != null)
                        DestroyImmediate(m_Cells[i, j].gameObject);
                }
            }

            m_Cells = new CellBehaviour[0, 0];
        }


        CellBehaviour CreateCell(int i, int j)
        {
            CellBehaviour cell = Instantiate(CellBehaviourPrefab);
            cell.transform.position = new Vector3(CellSize * i, 0, -CellSize * j);
            cell.gameObject.name = string.Format("Cell {0} {1}", i, j);
            cell.transform.parent = transform;
            m_Cells[i, j] = cell;

            return cell;
        }
    }
}