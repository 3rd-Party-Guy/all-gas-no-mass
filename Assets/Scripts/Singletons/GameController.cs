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
    private DifficultyController difficultyController;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerDeathPrefab;
    [SerializeField] private AudioClip playerDeathSound;
    
    private AudioSource audioSource;
    private GameObject player;
    private Transform levelStart;

    [SerializeField] private int levelAmountGoal;
    private int levelsCompleted;
    private int playerDeaths;

    public event EventHandler OnLevelComplete;
    public event EventHandler OnGameCompleted;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerRespawn;

    [SerializeField] private InputAction respawnPlayerAction;

    public bool isPlayerDead;

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
            
        respawnPlayerAction.performed += _ => RespawnPlayer(true);

        levelsCompleted = 0;
        playerDeaths = 0;

        mapGenerator.OnLevelGenerationComplete += RespawnPlayer;
    }

    private void RespawnPlayer(object e, EventArgs data) => RespawnPlayer(false);

    private void RespawnPlayer(bool isDeath = true) {
        if (isDeath) {
            playerDeaths++;

            AudioPlayer.PlayOneShot(playerDeathSound);
            OnPlayerRespawn?.Invoke(this, EventArgs.Empty);
            
            GameObject go = Instantiate(playerDeathPrefab);
            Destroy(go, 5);

            OnPlayerDeath?.Invoke(this, EventArgs.Empty);
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

        isPlayerDead = false;
    }

    private void SetupSubsingletons() {
        scoreSystem = GetComponent<ScoreSystem>();
        uiController = GetComponent<UIController>();
        interactableGenerator = GetComponent<InteractableGenerator>();
        difficultyController = GetComponent<DifficultyController>();

        GameObject mapGenObject = GameObject.FindGameObjectWithTag("LevelGenerator");
        mapGenerator = mapGenObject.GetComponent<MapGenerator>();
        meshGenerator = mapGenObject.GetComponent<MeshGenerator>();
    }

    public Vector3 GetFreePosition() {
        return meshGenerator.GetFreePosition();
    }

    public void CompleteLevel() {
        if (!scoreSystem.IsEnough())
            return;
            
        levelsCompleted++;

        if (levelsCompleted >= levelAmountGoal)
            OnGameCompleted?.Invoke(this, EventArgs.Empty);

        OnLevelComplete?.Invoke(this, EventArgs.Empty);
    }

    public void GameOver() {
        RespawnPlayer();
    }

    private void OnEnable() {
        respawnPlayerAction.Enable();
    }

    private void OnDisable() {
        respawnPlayerAction.Disable();
    }

    public int CompletedLevelsAmount {
        get => levelsCompleted;
    }

    public int PlayerDeaths {
        get => playerDeaths;
    }

    public ScoreSystem ScoreSystem {
        get => scoreSystem;
    }

    public AudioSource AudioPlayer {
        get => audioSource;
    }

    public GameObject Player {
        get => player;
    }

    public Transform PlayerTransform {
        get => player.transform;
    }

    public MapGenerator MapGen {
        get => mapGenerator;
    }
}
