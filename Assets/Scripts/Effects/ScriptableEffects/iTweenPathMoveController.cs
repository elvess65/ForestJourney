using UnityEngine;

public class iTweenPathMoveController : MonoBehaviour 
{
	public System.Action OnArrived;

    private iTweenPath m_PathController;

    void Start()
    {
        m_PathController = GetComponent<iTweenPath>();
    }

    public void StartMove(float speed)
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(m_PathController.pathName), 
                                              "speed", speed, 
                                              "easetype", iTween.EaseType.linear, 
                                              "oncomplete", "ArrivedHandler"));
    }

	void ArrivedHandler()
	{
		if (OnArrived != null)
			OnArrived();
	}
}
