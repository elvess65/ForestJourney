using UnityEngine;

namespace GridEditor
{
    public class CellBehaviour : MonoBehaviour
    {
        [Tooltip("Left, Right, Top, Bottom")]
        public GameObject[] ConnectionList;

        private CellData m_CellData;


        /// <summary>
        /// Задать данные ячейки
        /// </summary>
        public void SetCellData(CellData cellData)
        {
            m_CellData = new CellData(cellData.RootCell, cellData.LinkedCell);
        }

        /// <summary>
        /// Получить данные о ячейке
        /// </summary>
        /// <returns>The cell data.</returns>
        public CellData GetCellData()
        {
            return m_CellData;
        }


        /// <summary>
        /// Создать связь с ячейкой 
        /// </summary>
        /// <param name="linkedCell">Linked cell.</param>
        public void LinkCell(CellBehaviour linkedCell)
        {
            //Определить тип горизонтального направления к ячейке по вектору направления
            Vector3 dirToLinedCell = (transform.position - linkedCell.transform.position).normalized;
            GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinedCell);

            CreateLink(hDir, linkedCell.GetCellData().RootCell);
        }

        void CreateLink(GridTools.HorizontalDirections dir, Cell cell)
        {
            //Включить связь
            ConnectionList[(int)dir].gameObject.SetActive(true);

            //Данные о связанной ячейке
            m_CellData.LinkedCell = new Cell(cell);
        }
    }
}
