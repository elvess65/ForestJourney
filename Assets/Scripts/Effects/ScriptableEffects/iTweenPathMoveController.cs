using UnityEngine;

[RequireComponent(typeof(iTweenPath))]
public class iTweenPathMoveController : MonoBehaviour 
{
	public System.Action OnArrived;
    public iTween.EaseType EaseType = iTween.EaseType.linear;

    private iTweenPath m_PathController;

    void Start()
    {
        m_PathController = GetComponent<iTweenPath>();
    }

    public void StartMove(float speed)
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(m_PathController.pathName), 
                                              "speed", speed, 
                                              "easetype", EaseType, 
                                              "oncomplete", "ArrivedHandler"));
    }

    public void ChangeNode(int index, Vector3 pos)
    {
        try
        {
            iTweenPath.GetPath(m_PathController.pathName)[index] = pos;
        }
        catch(System.IndexOutOfRangeException)
        {
            Debug.LogError("Error gettings path with index " + index);
        }
    }

	void ArrivedHandler()
	{
		if (OnArrived != null)
			OnArrived();
	}
}
