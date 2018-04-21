using System.Collections.Generic;
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

    private PlayerController m_Player;
    private PrefabsLibrary m_PrefabsLibrary;
    private List<EnemyController> m_Enemies;

    private Dictionary<KeyController.KeyTypes, int> m_CollectedKeys;

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
        CreateMainEntities();
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

    public void AddKey(KeyController.KeyTypes type)
    {
        if (m_CollectedKeys == null)
            m_CollectedKeys = new Dictionary<KeyController.KeyTypes, int>();

        if (!m_CollectedKeys.ContainsKey(type))
            m_CollectedKeys.Add(type, 0);

        m_CollectedKeys[type]++;
    }

    public void RemoveKey(KeyController.KeyTypes type)
    {
        if (m_CollectedKeys == null)
            return;

        if (m_CollectedKeys.ContainsKey(type))
            m_CollectedKeys[type]--;

        if (m_CollectedKeys[type] <= 0)
            m_CollectedKeys.Remove(type);
    }

    public bool HasKeysForActivation(KeyController.KeyTypes[] keys)
    {
        bool result = false;
        if (m_CollectedKeys != null)
        {
            for (int i = 0; i < keys.Length; i++)
                result = m_CollectedKeys.ContainsKey(keys[i]);
        }

        return result;
    }

    void StartLoop()
    {
        m_IsActive = true;
        CameraController.Init(Player.transform);
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
        if (!m_IsActive && GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Start"))
        {
            CreatePlayer();
            StartLoop();
        }
    }
}
