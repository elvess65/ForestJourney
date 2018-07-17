using UnityEngine;

namespace GridEditor
{
    public class GridController : MonoBehaviour
    {
        public CellBehaviour CellBehaviourPrefab;
        [Header("Settings")]
        public int CellSize = 10;
        public int GridSize = 3;

        private CellBehaviour[,] m_Cells = null;

        public bool GridExists
        {
            get { return m_Cells != null && m_Cells.Length > 0; }
        }

        public void LoadGridData()
        {
            GridDataController dataController = GetComponent<GridDataController>();
            CellData[,] cellDataArr = dataController.GetData();
            m_Cells = new CellBehaviour[GridSize, GridSize];

            //Создать ячейки
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    CellBehaviour cell = CreateCell(i, j);
                    cell.SetCellData(cellDataArr[i, j]);
                }
            }

            //Создать связи между ячейками
            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
                {
                    //Если у ячейки есть связи
                    if (m_Cells[i, j].GetCellData().LinkedCell != null)
                    {
                        //Координаты связанной ячейке
                        int x = m_Cells[i, j].GetCellData().LinkedCell.X;
                        int y = m_Cells[i, j].GetCellData().LinkedCell.Y;

                        //Связать ячейки
                        m_Cells[i, j].LinkCell(m_Cells[x, y]);
                    }
                }
            }
        }

        public void SaveGridData()
        {
            GridDataController dataController = GetComponent<GridDataController>();
            dataController.SaveData(m_Cells, GridSize);
        }

        public void CreateDefaultGrid()
        {
            m_Cells = new CellBehaviour[GridSize, GridSize];

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
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

            for (int i = 0; i < GridSize; i++)
            {
                for (int j = 0; j < GridSize; j++)
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