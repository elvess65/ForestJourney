﻿using UnityEngine;

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
        CameraController.Init(GameState.Player.transform);
    }

    void CreateMainEntities()
    {
        m_PrefabsLibrary = Instantiate(PrefabsLibraryPrefab);
    }

    void CreatePlayer()
    {
        GameState.Player = Instantiate(m_PrefabsLibrary.PlayerPrefab, PlayerSpawnPoint.position, Quaternion.identity);
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
