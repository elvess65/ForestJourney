using System.Collections.Generic;
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
    private UIManager m_UIManager;
    private PlayerController m_Player;
    private PrefabsLibrary m_PrefabsLibrary;
    private List<EnemyController> m_Enemies;
    
    public bool IsActive
    {
        get { return m_IsActive; }
    }
    public UIManager UIController;
    public PlayerController Player
    {
        get { return m_Player; }
    }
    public PrefabsLibrary PrefabLibrary
    {
        get { return m_PrefabsLibrary; }
    }
    public List<EnemyController> Enemies
    {
        get
        {
            if (m_Enemies == null)
                m_Enemies = new List<EnemyController>();

            return m_Enemies;
        }
    }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        m_UIManager = GetComponent<UIManager>();

        CreateMainEntities();
        CreatePlayer();
    }

    public void AddEnemy(EnemyController enemy)
    {
        if (m_Enemies == null)
            m_Enemies = new List<EnemyController>();

        m_Enemies.Add(enemy);
    }

    public void FinishRound()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void RestartRound()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
