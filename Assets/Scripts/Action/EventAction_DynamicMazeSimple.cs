using UnityEngine;

public class EventAction_DynamicMazeSimple : MonoBehaviour
{
    public GameObject DynamicMazeObject;

	void Start ()
    {
        //GetComponent<Action_Base>().OnAction += GenerateMaze;
    }

    public void GenerateMaze()
    {
        DynamicMazeObject.SetActive(true);
        GameManager.Instance.CamController.RotateRandomly();
    }
}
