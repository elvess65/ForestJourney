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
        public CellConnection[] ConnectionList;

        private CellData m_CellData;
        private Renderer m_Renderer;

        /// <summary>
        /// Задать данные ячейки
        /// </summary>
        public void SetCellData(CellData cellData)
        {
            m_CellData = new CellData(cellData.RootCell, cellData.VerticalLevel, cellData.LinkedCells);
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
		/// <param name="linkedCell">Связаная ячейка</param>
        /// <param name="addCellToConnection">Добавить ячейку в связи (редактор) <c>true</c>, 
        /// иначе не добавлять ячейку в связи (загрущка)</param>
		public void LinkCell(CellBehaviour linkedCell, bool addCellToConnection = true)
        {
            //Определить тип горизонтального направления к ячейке по вектору направления
            Vector3 dirToLinkedCell = (transform.position - linkedCell.transform.position).normalized;
            GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinkedCell);

            CreateLink(hDir, linkedCell.GetCellData().RootCell, addCellToConnection);

			//Обновить высоту всем соединениям конкретной ячейки
			UpdateVerticalDirectionForConnections(true);
        }

		/// <summary>
		/// Удалить связь с ячейкой
		/// </summary>
		/// <param name="linkedCell">Связаная ячейка</param>
		public void UnlinkCell(CellBehaviour linkedCell)
        {
            //Определить тип горизонтального направления к ячейке по вектору направления
            Vector3 dirToLinkedCell = (transform.position - linkedCell.transform.position).normalized;
            GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinkedCell);

            RemoveLink(hDir, linkedCell.GetCellData().RootCell);
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

        void RemoveLink(GridTools.HorizontalDirections dir, Cell cell)
        {
            //Выключить связь
            ConnectionList[(int)dir].gameObject.SetActive(false);

            //Удалить данные о связанной ячейке
            m_CellData.RemoveLinkedCell(cell);

			UpdateEnviroment();
        }


        /// <summary>
        /// Поднять ячейку вверх (только редактор)
        /// </summary>
        public void MoveCellHigher(float step)
        {
			//Поднять или опустить ячейку
			IncrementCellVerticalPosition(step);
			//Обновить высоту всем соединениям конкретной ячейки
			UpdateVerticalDirectionForConnections(true);

			//Сохранить новый уровень ячейки
			GridController gridController = FindObjectOfType<GridController>();
            m_CellData.VerticalLevel = (int)(transform.position.y / gridController.VerticalStep);
        }

		/// <summary>
        /// Опустить ячейку вниз (только редактор)
        /// </summary>
		public void MoveCellLower(float step)
        {
			//Поднять или опустить ячейку
			IncrementCellVerticalPosition(-step);
			//Обновить высоту всем соединениям конкретной ячейки
			UpdateVerticalDirectionForConnections(true);

            //Сохранить новый уровень ячейки
			GridController gridController = FindObjectOfType<GridController>();
            m_CellData.VerticalLevel = (int)(transform.position.y / gridController.VerticalStep);
        }

		/// <summary>
		/// Обновить высоту всем соединениям конкретной ячейки
		/// </summary>
        /// <param name="updateConnectionForLinkedCell">Обновить соеинение связанным ячейкам (редактор) если <c>true</c>,
        /// иначе не обновлять состояние связанным ячейкам (загрузка).</param>
		public void UpdateVerticalDirectionForConnections(bool updateConnectionForLinkedCell)
        {
			GridController gridController = FindObjectOfType<GridController>();
            //Все связанные ячейки
			foreach (Cell cData in m_CellData.LinkedCells)
			{
				CellBehaviour linkedCellBehaviour = gridController.GetCell(cData.X, cData.Y);

                //Обновить состояние соединения
				if (linkedCellBehaviour != null)
                    SetVerticalDirectionToLinkedCell(linkedCellBehaviour);

                //Обновить высоту соединения соседней ячейки (редактор)
                if (updateConnectionForLinkedCell)
                    linkedCellBehaviour.UpdateVerticalDirectionForConnections(false);
			}
        }

		/// <summary>
		/// Задать высоту соединения с конкретной ячейкой
		/// </summary>
		void SetVerticalDirectionToLinkedCell(CellBehaviour linkedCellBehaviour)
		{
            //Направление к ячейке
			Vector3 dirToLinkedCell = (transform.position - linkedCellBehaviour.transform.position).normalized;

            //Направление по вертикали и по горизонтали
			GridTools.HorizontalDirections hDir = GridTools.GetHorizontalDirectionByVector(dirToLinkedCell);
			GridTools.VerticalDirections vDir = GridTools.GetVerticalDirectionByVector(dirToLinkedCell);

            //Изменить состояние направления соединения
            ConnectionList[(int)hDir].SetVerticaDirection(vDir);
		}

		/// <summary>
		/// Поднять или опустить ячейку
		/// </summary>
		void IncrementCellVerticalPosition(float step)
        {
            Vector3 pos = transform.position;
            pos.y += step;
            transform.position = pos;
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