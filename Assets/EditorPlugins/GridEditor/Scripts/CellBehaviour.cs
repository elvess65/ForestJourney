using UnityEngine;

namespace GridEditor
{
    public class CellBehaviour : MonoBehaviour
    {
		public static bool EDIT_MODE = false;
		public static bool PREV_EDIT_MODE = false;
		public static bool LINK_MODE = false;
		public static bool UNLINK_MODE = false;
		public static CellBehaviour PrevTargetCell;

		public System.Action<CellBehaviour> OnCellSelected;
		public System.Action<bool> OnEditModeChanged;

        public GameObject Enviroment;
        [Tooltip("Left, Right, Top, Bottom")]
        public GameObject[] ConnectionList;

        private CellData m_CellData;
        private Renderer m_Renderer;

        /// <summary>
        /// Задать данные ячейки
        /// </summary>
        public void SetCellData(CellData cellData)
        {
            m_CellData = new CellData(cellData.RootCell, cellData.LinkedCells);
        }

        /// <summary>
        /// Получить данные о ячейке
        /// </summary>
        /// <returns>The cell data.</returns>
        public CellData GetCellData()
        {
            return m_CellData;
        }

        public void ColorizeCell(Material mat)
        {
            if (m_Renderer == null)
                m_Renderer = GetComponent<MeshRenderer>();

            if (m_Renderer.sharedMaterial.color != mat.color)
            {
                m_Renderer.sharedMaterial = mat;
            }
        }

        public void UpdateEnviroment()
        {
            Enviroment.gameObject.SetActive(m_CellData.LinkedCells.Count > 0);
        }

        /// <summary>
        /// Создать связь с ячейкой 
        /// </summary>
        /// <param name="linkedCell">Linked cell.</param>
        public void LinkCell(CellBehaviour linkedCell, bool addCellToConnection = true)
        {
            //Определить тип горизонтального направления к ячейке по вектору направления
            Vector3 dirToLinedCell = (transform.position - linkedCell.transform.position).normalized;
            GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinedCell);

            CreateLink(hDir, linkedCell.GetCellData().RootCell, addCellToConnection);
        }

        void CreateLink(GridTools.HorizontalDirections dir, Cell cell, bool addCellToConnection)
        {
            //Включить связь
            ConnectionList[(int)dir].gameObject.SetActive(true);

            //Данные о связанной ячейке
            if (addCellToConnection)
                m_CellData.AddLinkedCell(new Cell(cell));

            UpdateEnviroment();
        }


        public void UnlinkCell(CellBehaviour linkedCell)
        {
			//Определить тип горизонтального направления к ячейке по вектору направления
			Vector3 dirToLinedCell = (transform.position - linkedCell.transform.position).normalized;
			GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinedCell);

            RemoveLink(hDir, linkedCell.GetCellData().RootCell);
        }

        void RemoveLink(GridTools.HorizontalDirections dir, Cell cell)
        {
			//Выключить связь
            ConnectionList[(int)dir].gameObject.SetActive(false);

			//Удалить данные о связанной ячейке
			m_CellData.RemoveLinkedCell(cell);
        }


        public override bool Equals(object other)
        {
            CellBehaviour cell = (CellBehaviour)other;
            if (cell == null)
                return false;

            return GetCellData().RootCell.Equals(cell.GetCellData().RootCell);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
