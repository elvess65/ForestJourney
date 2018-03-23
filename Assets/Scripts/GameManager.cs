using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Prefabs")]
    public PrefabsLibrary PrefabsLibraryPrefab;
    [Header("References")]
    public CameraController CamController;
    public Transform PlayerSpawnPoint;

    private bool m_IsActive = false;
    private PlayerController m_Player;
    private PrefabsLibrary m_PrefabsLibrary;
    
    public bool IsActive
    {
        get { return m_IsActive; }
    }
    public PlayerController Player
    {
        get { return m_Player; }
    }
    public PrefabsLibrary PrefabLibrary
    {
        get { return m_PrefabsLibrary; }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateMainEntities();
        CreatePlayer();
    }

    void StartLoop()
    {
        m_IsActive = true;
        CamController.Init(Player.transform);
    }

    void CreateMainEntities()
    {
        m_PrefabsLibrary = Instantiate(PrefabsLibraryPrefab);
    }

    void CreatePlayer()
    {
        m_Player = Instantiate(m_PrefabsLibrary.PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
    }

    private void OnGUI()
    {
        if (!m_IsActive && GUI.Button(new Rect(Screen.width / 2, Screen.height/ 2, 200, 100), "Start"))
            StartLoop();
    }
}
