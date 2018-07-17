using UnityEngine;

namespace GridEditor
{
    public class GridTools
    {
        public enum HorizontalDirections
        {
            Left, Right, Top, Bottom
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
    }

    /// <summary>
    /// Данные о ячейке (Координаты и связи)
    /// </summary>
    [System.Serializable]
    public class CellData
    {
        public Cell RootCell;
        public Cell LinkedCell;

        public CellData(Cell rootCell, Cell linkedCell)
        {
            RootCell = rootCell;
            LinkedCell = linkedCell;
        }

        public CellData(Cell rootCell)
        {
            RootCell = rootCell;
            LinkedCell = null;
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
    }
}