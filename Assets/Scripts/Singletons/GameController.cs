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

    private MapGenerator mapGenerator;
    private MeshGenerator meshGenerator;

    private InteractableGenerator interactableGenerator;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerDeathPrefab;
    [SerializeField] private AudioClip playerDeathSound;
    
    AudioSource audioSource;
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
            audioSource = GetComponent<AudioSource>();

            return;
        }
        
        Destroy(this);
    }

    private void Start() {
        GameObject levelStartObj = GameObject.FindGameObjectWithTag("LevelStart");
        if (levelStartObj == null)
            levelStart = null;
        else
            levelStart = levelStartObj.transform;
            
        RespawnPlayer(false);

        respawnPlayerAction.performed += _ => RespawnPlayer(true);

        interactableGenerator.GenerateInteractables();
    }

    private void RespawnPlayer(bool isRestart = true) {
        if (isRestart) {
            AudioPlayer.PlayOneShot(playerDeathSound);
            OnPlayerRespawn?.Invoke(this, EventArgs.Empty);
            
            GameObject go = Instantiate(playerDeathPrefab);
            Destroy(go, 5);
        }
        Destroy(player);
        
        player = Instantiate(playerPrefab);
        if (levelStart == null)
            player.transform.position = meshGenerator.GetFreePosition();
        else {
            player.transform.position = levelStart.position;
            player.transform.rotation = levelStart.transform.rotation;
        }

        Camera.main.GetComponent<CameraMovement>().Target = player.transform;
    }

    private void SetupSubsingletons() {
        scoreSystem = GetComponent<ScoreSystem>();
        uiController = GetComponent<UIController>();
        interactableGenerator = GetComponent<InteractableGenerator>();

        GameObject mapGenObject = GameObject.FindGameObjectWithTag("LevelGenerator");
        mapGenerator = mapGenObject.GetComponent<MapGenerator>();
        meshGenerator = mapGenObject.GetComponent<MeshGenerator>();
    }

    public Vector3 GetFreePosition() {
        return meshGenerator.GetFreePosition();
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

    public AudioSource AudioPlayer {
        get => audioSource;
    }
}
