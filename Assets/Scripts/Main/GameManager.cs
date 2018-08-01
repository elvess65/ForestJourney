using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Prefabs")]
    public PrefabsLibrary PrefabsLibraryPrefab;
    [Header("References")]
    public UIManager UIManager;
    public QueueAssistManager AssistManager;
    public CameraController CameraController;
    public GameStateController GameState;
    public Transform PlayerSpawnPoint;
    
    private bool m_IsActive = false;
    private PrefabsLibrary m_PrefabsLibrary;

    public bool IsActive
    {
        get { return m_IsActive; }
    }
    public PrefabsLibrary PrefabLibrary
    {
        get { return m_PrefabsLibrary; }
    }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        CreateMainEntities();
		CreatePlayer();
		StartLoop();
    }

    public void FinishRound()
    {
        LevelLoader.Instance.LoadNextLevel();
    }

    public void RestartRound()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    void StartLoop()
    {
        CameraController.OnCameraArrived += () => InputManager.Instance.InputIsEnabled = true;
        CameraController.Init(GameState.Player.transform);

        m_IsActive = true;
    }

    void CreateMainEntities()
    {
        m_PrefabsLibrary = Instantiate(PrefabsLibraryPrefab);
    }

    void CreatePlayer()
    {
        GameState.Player = Instantiate(m_PrefabsLibrary.PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
    }
}
