using System.Collections.Generic;
using UnityEngine;

namespace GridEditor
{
    public class GridTools
    {
        public enum HorizontalDirections
        {
            Left, Right, Top, Bottom
        }

        public enum VerticalDirections
        {
            Higher, Lower, Same
        }

        public static HorizontalDirections GetOppositeHorizontalDirection(HorizontalDirections dir)
        {
            HorizontalDirections result = HorizontalDirections.Left;
            switch (dir)
            {
                case HorizontalDirections.Left:
                    result = HorizontalDirections.Right;
                    break;
                case HorizontalDirections.Right:
                    result = HorizontalDirections.Left;
                    break;
                case HorizontalDirections.Top:
                    result = HorizontalDirections.Bottom;
                    break;
                case HorizontalDirections.Bottom:
                    result = HorizontalDirections.Top;
                    break;
            }

            return result;
        }

        public static HorizontalDirections GetHorizontalDirectionByVector(Vector3 dir)
        {
            HorizontalDirections result = HorizontalDirections.Top;
            if (dir.x > 0)
                result = HorizontalDirections.Left;
            else if (dir.x < 0)
                result = HorizontalDirections.Right;
            else if (dir.z > 0)
                result = HorizontalDirections.Bottom;
            else if (dir.z < 0)
                result = HorizontalDirections.Top;

            return result;
        }

        public static VerticalDirections GetOppositeVericalDirection(VerticalDirections dir)
        {
            VerticalDirections result = VerticalDirections.Same;
            switch (dir)
            {
                case VerticalDirections.Higher:
                    result = VerticalDirections.Lower;
                    break;
                case VerticalDirections.Lower:
                    result = VerticalDirections.Higher;
                    break;
                case VerticalDirections.Same:
                    result = VerticalDirections.Same;
                    break;
            }

            return result;
        }

        public static VerticalDirections GetVerticalDirectionByVector(Vector3 dir)
        {
            VerticalDirections result = VerticalDirections.Same;
            if (dir.y > 0)
                result = VerticalDirections.Lower;
            else if (dir.y < 0)
                result = VerticalDirections.Higher;

            return result;
        }
    }

    /// <summary>
    /// Данные о ячейке (Координаты и связи)
    /// </summary>
    [System.Serializable]
    public class CellData
    {
        public int VerticalLevel;
        public Cell RootCell;
        public List<Cell> LinkedCells;

        //Для сериалализации
        public CellData()
        {
            RootCell = null;
            LinkedCells = null;
            VerticalLevel = 0;
        }

        //Для создания с сохранения
        public CellData(Cell rootCell, int vLevel, List<Cell> linkedCells)
        {
            VerticalLevel = vLevel;
            RootCell = rootCell;
            LinkedCells = new List<Cell>(linkedCells);
        }

        //Для создания сетки по-умолчанию
        public CellData(Cell rootCell)
        {
            VerticalLevel = 0;
            RootCell = rootCell;
            LinkedCells = new List<Cell>();
        }

        public void AddLinkedCell(Cell linkedCell)
        {
            LinkedCells.Add(linkedCell);
        }

        public void RemoveLinkedCell(Cell linkedCell)
        {
            LinkedCells.Remove(linkedCell);
        }
    }

    /// <summary>
    /// Данные о координатах ячейки
    /// </summary>
    [System.Serializable]
    public class Cell
    {
        public int X;
        public int Y;

        public Cell()
        {
            X = 0;
            Y = 0;
        }

        public Cell(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Cell(Cell cell)
        {
            X = cell.X;
            Y = cell.Y;
        }

        public override bool Equals(object obj)
        {
            Cell cell = (Cell)obj;

            if (cell == null)
                return false;

            return X == cell.X && Y == cell.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}