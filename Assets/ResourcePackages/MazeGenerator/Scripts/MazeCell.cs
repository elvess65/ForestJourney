using UnityEngine;
using System.Collections;

public enum Direction{
	Start,
	Right,
	Front,
	Left,
	Back,
};
//<summary>
//Class for representing concrete maze cell.
//</summary>
public class MazeCell 
{
    public MazeCell(int column, int row)
    {
        Column = column;
        Row = row;
    }

	public bool IsVisited = false;
	public bool WallRight = false;
	public bool WallFront = false;
	public bool WallLeft = false;
	public bool WallBack = false;
	public bool IsGoal = false;
    public int Row;
    public int Column;

    public override string ToString()
    {
        return string.Format("[MazeCell] Column: {0} Row: {1}", Column, Row);
    }
}
