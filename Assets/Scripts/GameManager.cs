using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public CameraController CamController;
    public PlayerController Player;

    private bool m_IsActive = false;

    public bool IsActive
    {
        get { return m_IsActive; }
    }

    private void Awake()
    {
        Instance = this;
    }

    void StartLoop()
    {
        m_IsActive = true;
        CamController.Init(Player.transform);
    }

    private void OnGUI()
    {
        if (!m_IsActive && GUI.Button(new Rect(Screen.width / 2, Screen.height/ 2, 200, 100), "Start"))
            StartLoop();
    }
}
