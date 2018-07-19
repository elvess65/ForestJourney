using UnityEditor;
using UnityEngine;

namespace GridEditor
{
    [CustomEditor(typeof(CellBehaviour))]
    public class CellBehaviourEditor : Editor
    {
        private Color m_NormalColor = Color.white;
        private Color m_EditModeColor = Color.green;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //Цвет кнопки
            GUI.backgroundColor = CellBehaviour.LINK_MODE ? m_EditModeColor : m_NormalColor;
            if (GUILayout.Button(LinkButtonName()))
            {
                //Вход/выход в режим редактирования
                CellBehaviour.EDIT_MODE = !CellBehaviour.EDIT_MODE;
                CellBehaviour.LINK_MODE = !CellBehaviour.LINK_MODE;

                //Если включен режим редактирования
                if (CellBehaviour.EDIT_MODE)
                {
                    //Подписаться на событие выделения ячейки
					CellBehaviour curCell = (CellBehaviour)target;
                    curCell.OnCellSelected = LinkCell;
                }
            }

			//Цвет кнопки
			GUI.backgroundColor = CellBehaviour.UNLINK_MODE ? m_EditModeColor : m_NormalColor;
            if (GUILayout.Button(UnlinkButtonName()))
            {
				//Вход/выход в режим редактирования
				CellBehaviour.EDIT_MODE = !CellBehaviour.EDIT_MODE;
                CellBehaviour.UNLINK_MODE = !CellBehaviour.UNLINK_MODE;

				//Если включен режим редактирования
				if (CellBehaviour.EDIT_MODE)
                {
					//Подписаться на событие выделения ячейки
					CellBehaviour curCell = (CellBehaviour)target;
                    curCell.OnCellSelected = UnlinkCell;
                }
            }
        }

        void OnEnable()
        {
            //Подписаться на событие входа/выхода из режима редактирования
            CellBehaviour source = (CellBehaviour)target;
            if (source.OnEditModeChanged == null)
                source.OnEditModeChanged = EditModeChangedHandler;
        }

        void OnSceneGUI()
		{
            //Отслеживание состояния изменения редима редактирования для текущей ячейки
            if (CellBehaviour.EDIT_MODE.Equals(!CellBehaviour.PREV_EDIT_MODE))
            {
                CellBehaviour.PREV_EDIT_MODE = CellBehaviour.EDIT_MODE;

                CellBehaviour curCell = (CellBehaviour)target;
                curCell.OnEditModeChanged(CellBehaviour.EDIT_MODE);
            }

            //Выделение ячейки на которую наведен указатель
            if (CellBehaviour.EDIT_MODE)
            {
                CellBehaviour targetCell = GetCell();
                if (targetCell != null)
                {
                    CellBehaviour curCell = (CellBehaviour)target;

                    //Если выделеная ячейка не текущая
                    if (!curCell.Equals(targetCell))
                    {
                        GridController gridController = FindObjectOfType<GridController>();

                        //Восстановить цвет предыдущей ячейке
                        if (CellBehaviour.PrevTargetCell != null)
                            CellBehaviour.PrevTargetCell.ColorizeCell(gridController.MaterialNormalCell);

                        //Предыдущая ячейка равняеться текущей
                        CellBehaviour.PrevTargetCell = targetCell;

                        //Покрасить текущю ячейку
                        targetCell.ColorizeCell(gridController.MaterialTargetCell);

                        //Нарисовать линию к выбранной ячейке
                        Handles.DrawLine(curCell.transform.position, targetCell.transform.position);
                    }
                }
            }

            //Связка ячеек
			CellBehaviour linkedCell = GetCellByClick();
            if (linkedCell != null)
            {
                CellBehaviour curCell = (CellBehaviour)target;
                curCell.OnCellSelected(linkedCell);
            }
		}

     
        void LinkCell(CellBehaviour linkedCell)
        {
			CellBehaviour curCell = (CellBehaviour)target;

            if (!curCell.GetCellData().RootCell.Equals(linkedCell.GetCellData().RootCell) &&    //Если хотим привязать эту же ячейку
                !curCell.GetCellData().LinkedCells.Contains(linkedCell.GetCellData().RootCell)) //Если эта ячейка уже привязана
            {
                //Связать текущую ячейку с выбранной
                curCell.LinkCell(linkedCell);

                //Связать выбраную ячейку с текущей
                linkedCell.LinkCell(curCell);
            }
        }

        void UnlinkCell(CellBehaviour linkedCell)
        {
			CellBehaviour curCell = (CellBehaviour)target;

            if (!curCell.GetCellData().RootCell.Equals(linkedCell.GetCellData().RootCell) &&    //Если хотим отвязать эту же ячейку
                curCell.GetCellData().LinkedCells.Contains(linkedCell.GetCellData().RootCell))  //Если эта ячейка связана
            {
				//Отвязать текущую ячейку от выбранной
				curCell.UnlinkCell(linkedCell);

				//Отвязать выбраную ячейку от текущей
				linkedCell.UnlinkCell(curCell);
            }
        }


        void EditModeChangedHandler(bool isEditMode)
        {
            CellBehaviour curCell = (CellBehaviour)target;
            GridController gridController = FindObjectOfType<GridController>();

            if (!isEditMode) //Если режим редактрирования выключен
            {
                //Выключеть текущий режим
                CellBehaviour.LINK_MODE = false;
                CellBehaviour.UNLINK_MODE = false;

                //Обновить цвета всех ячеек до изначального
                gridController.ResetColor();
            }
            else
            {
                //Цвет текущей ячейки
                curCell.ColorizeCell(gridController.MaterialSelectedCell);
            }
        }


        string LinkButtonName()
        {
            return !CellBehaviour.LINK_MODE ? "Start link" : "Stop Link";
        }

        string UnlinkButtonName()
        {
            return !CellBehaviour.UNLINK_MODE ? "Start Unlink" : "Stop Unlink";
        }

		CellBehaviour GetCellByClick()
		{
			if (CellBehaviour.EDIT_MODE && Event.current.type == EventType.MouseDown)
			{
				CellBehaviour.EDIT_MODE = false;
                return GetCell();
			}

			return null;
		}

        CellBehaviour GetCell()
        {
			RaycastHit hit;
			Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

			if (Physics.Raycast(ray, out hit))
				return hit.collider.GetComponent<CellBehaviour>();

            return null;
        }
    }
}
