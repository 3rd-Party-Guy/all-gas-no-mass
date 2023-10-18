using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private ScoreSystem scoreSystem;
    private UIController uiController;

    [SerializeField] private GameObject playerPrefab;
    GameObject player;
    GameStateType gameState;
    Transform levelStart;

    public event EventHandler OnLevelComplete;
    public event EventHandler OnGameStateChange;
    public event EventHandler OnPlayerRespawn;

    [SerializeField] private InputAction respawnPlayerAction;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            SetupSubsingletons();

            player = GameObject.FindGameObjectWithTag("Player");

            return;
        }
        
        Destroy(this);
    }

    private void Start() {
        levelStart = GameObject.FindGameObjectWithTag("LevelStart").transform;
        RespawnPlayer(false);

        respawnPlayerAction.performed += _ => RespawnPlayer(true);
    }

    private void Update() {
    }

    private void RespawnPlayer(bool isRestart = true) {
        if (isRestart)
            OnPlayerRespawn?.Invoke(this, EventArgs.Empty);
        Destroy(player);
        
        player = Instantiate(playerPrefab);
        player.transform.position = levelStart.transform.position;
        player.transform.rotation = levelStart.transform.rotation;

        Camera.main.GetComponent<CameraMovement>().Target = player.transform;
    }

    private void SetupSubsingletons() {
        scoreSystem = GetComponent<ScoreSystem>();
    }

    public void CompleteLevel() {
        OnLevelComplete?.Invoke(this, EventArgs.Empty);
    }

    public void GameOver() {
        GameState = GameStateType.GameOver;
        RespawnPlayer();
    }

    private void OnEnable() {
        respawnPlayerAction.Enable();
    }

    private void OnDisable() {
        respawnPlayerAction.Disable();
    }

    public GameStateType GameState {
        get => gameState;
        private set {
            gameState = value;
            OnGameStateChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public ScoreSystem ScoreSystem {
        get => scoreSystem;
    }
}
