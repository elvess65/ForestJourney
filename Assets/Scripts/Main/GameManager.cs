using mytest.CameraSystem;
using mytest.Main;
using mytest.Main.MiniGames;
using mytest.UI;
using mytest.UI.InputSystem;
using mytest.UI.Loading;
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
    public MiniGameController MiniGameController;

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
        m_IsActive = false;

        GameState.GameOver();
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

/*
 * Дизайн 2 и 3-го уровней
 * 4-ый уровень с прыжками на платформы и головоломкой
 * Убрать с обучения подсветку при затемнении
 * Текстура тотемов
 * Анимации взаимодейтсвия
 * Кнопка для прыжка 
 * - Кнопка
 * - Показ кнопки при необходимости
 * - Откат кнопки
 */
