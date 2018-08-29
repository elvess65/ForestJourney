using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour {
	public enum MazeGenerationAlgorithm{
		PureRecursive,
		RecursiveTree,
		RandomTree,
		OldestTree,
		RecursiveDivision,
	}

	public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
	public bool FullRandom = false;
	public int RandomSeed = 12345;
	public GameObject Floor = null;
	public GameObject Wall = null;
	public GameObject Pillar = null;
    public GameObject Enemy = null;
	public int Rows = 5;
	public int Columns = 5;
	public float CellWidth = 5;
	public float CellHeight = 5;
	public bool AddGaps = true;
	public GameObject GoalPrefab = null;
    public bool Load = false;

	private BasicMazeGenerator mMazeGenerator = null;

    public MazeCell GetCellByPosition(Vector3 pos)
    {
		int column = Mathf.RoundToInt(pos.x / (CellWidth + (AddGaps ? .2f : 0)));
		int row = Mathf.RoundToInt(pos.z / (CellHeight + (AddGaps ? .2f : 0)));

        return mMazeGenerator.GetMazeCell(row, column);
    }

	void Start () 
    {
		if (!FullRandom)
        {
			Random.seed = RandomSeed;
		}

		switch (Algorithm) 
        {
    		case MazeGenerationAlgorithm.PureRecursive:
    			mMazeGenerator = new RecursiveMazeGenerator (Rows, Columns);
    			break;
    		case MazeGenerationAlgorithm.RecursiveTree:
    			mMazeGenerator = new RecursiveTreeMazeGenerator (Rows, Columns);
    			break;
    		case MazeGenerationAlgorithm.RandomTree:
    			mMazeGenerator = new RandomTreeMazeGenerator (Rows, Columns);
    			break;
    		case MazeGenerationAlgorithm.OldestTree:
    			mMazeGenerator = new OldestTreeMazeGenerator (Rows, Columns);
    			break;
    		case MazeGenerationAlgorithm.RecursiveDivision:
    			mMazeGenerator = new DivisionMazeGenerator (Rows, Columns);
    			break;
		}

		mMazeGenerator.GenerateMaze ();

        if (Load)
        {
            //PlayerPrefs.DeleteKey("str");
            List<MazeCell> visitedCells = new List<MazeCell>();

            string str = PlayerPrefs.GetString("str", string.Empty);
            if (!str.Equals(string.Empty))
            {
                string[] parsedStr = str.Split('|');
                for (int i = 0; i < parsedStr.Length; i++)
                {
                    string[] cellData = parsedStr[i].Split('.');
                    int column = int.Parse(cellData[0]);
                    int row = int.Parse(cellData[1]);

                    visitedCells.Add(new MazeCell(column, row));
                }
            }

            if (visitedCells.Count > 0)
            {
                for (int i = 0; i < visitedCells.Count; i++)
                {
                    float x = visitedCells[i].Column * (CellWidth + (AddGaps ? .2f : 0));
                    float z = visitedCells[i].Row * (CellHeight + (AddGaps ? .2f : 0));
                    MazeCell cell = mMazeGenerator.GetMazeCell(visitedCells[i].Row, visitedCells[i].Column);
                    GameObject tmp;

                    tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                    tmp.transform.parent = transform;

                    if (cell.WallRight)
                    {
                        tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallFront)
                    {
                        tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallLeft)
                    {
                        tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                        tmp.transform.parent = transform;
                    }
                    if (cell.WallBack)
                    {
                        tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                        tmp.transform.parent = transform;
                    }
                    if (cell.IsGoal && GoalPrefab != null)
                    {
                        bool isEnemy = Random.Range(0, 100) > 50;

                        tmp = Instantiate(isEnemy ? Enemy : GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                        tmp.transform.parent = transform;
                    }
                }
            }
        }
        else 
        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
				float x = column * (CellWidth + (AddGaps ? .2f : 0));
                float z = row * (CellHeight + (AddGaps ? .2f : 0));
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;

				tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
				tmp.transform.parent = transform;

				if (cell.WallRight)
                {
					tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position,Quaternion.Euler(0, 90, 0)) as GameObject;// right
					tmp.transform.parent = transform;
				}
				if (cell.WallFront)
                {
					tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position,Quaternion.Euler(0, 0, 0)) as GameObject;// front
					tmp.transform.parent = transform;
				}
				if (cell.WallLeft)
                {
					tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position,Quaternion.Euler(0, 270, 0)) as GameObject;// left
					tmp.transform.parent = transform;
				}
				if (cell.WallBack)
                {
					tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position,Quaternion.Euler(0, 180, 0)) as GameObject;// back
					tmp.transform.parent = transform;
				}
				if (cell.IsGoal && GoalPrefab != null)
                {
                    bool isEnemy = Random.Range(0, 100) > 50;

                    tmp = Instantiate(isEnemy ? Enemy : GoalPrefab, new Vector3(x, 1, z), Quaternion.Euler(0, 0, 0)) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}

		if (Pillar != null)
        {
			for (int row = 0; row < Rows + 1; row++) 
            {
				for (int column = 0; column < Columns + 1; column++) 
                {
					float x = column * (CellWidth + (AddGaps ? .2f : 0));
					float z = row * (CellHeight + (AddGaps ? .2f : 0));
					GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
					tmp.transform.parent = transform;
				}
			}
		}
	}
}
