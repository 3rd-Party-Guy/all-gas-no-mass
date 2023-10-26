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
    private Timer timer;

    private MapGenerator mapGenerator;
    private MeshGenerator meshGenerator;
    private DifficultyController difficultyController;

    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerDeathPrefab;
    [SerializeField] private AudioClip playerDeathSound;
    [SerializeField] private GameObject goalTracer;
    
    private AudioSource audioSource;
    private GameObject player;
    private Transform levelStart;

    [SerializeField] private int levelAmountGoal;
    private int levelsCompleted;
    private int playerDeaths;

    public event EventHandler OnLevelComplete;
    public event EventHandler OnGameComplete;
    public event EventHandler OnPlayerDeath;
    public event EventHandler OnPlayerRespawn;

    [SerializeField] private InputAction respawnPlayerAction;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            SetupSubsingletons();
            SubsribeEvents();

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

        mapGenerator.CreateNewWorld();
        goalTracer.SetActive(true);
    }

    private void OnGameCompleted(object e, EventArgs data) {

    }

    private void SubsribeEvents() {
        OnGameComplete += OnGameCompleted;
        mapGenerator.OnLevelGenerationComplete += OnLevelGenerationCompleted;
        OnLevelComplete += OnLevelCompleted;
    }

    private void OnLevelGenerationCompleted(object e, EventArgs data) {
        RespawnPlayer(false);
    }

    private void OnLevelCompleted(object e, EventArgs data) {
        difficultyController.ManageMapSize();
    }

    private void RespawnPlayer(object e, EventArgs data) => RespawnPlayer(false);

    private void RespawnPlayer(bool isDeath = true) {
        if (isDeath) {
            playerDeaths++;

            AudioPlayer.PlayOneShot(playerDeathSound);
            OnPlayerRespawn?.Invoke(this, EventArgs.Empty);
            
            GameObject go = Instantiate(playerDeathPrefab);
            go.transform.position = player.transform.position;
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
        difficultyController.ManageMovement();
    }

    private void SetupSubsingletons() {
        scoreSystem = GetComponent<ScoreSystem>();
        uiController = GetComponent<UIController>();
        timer = GetComponent<Timer>();
        audioSource = GetComponent<AudioSource>();
        difficultyController = GetComponent<DifficultyController>();

        GameObject mapGenObject = GameObject.FindGameObjectWithTag("LevelGenerator");
        mapGenerator = mapGenObject.GetComponent<MapGenerator>();
        meshGenerator = mapGenObject.GetComponent<MeshGenerator>();
    }

    public Vector3 GetFreePosition() {
        return meshGenerator.GetFreePosition();
    }

    public void CompleteLevel() {        
        levelsCompleted++;

        if (levelsCompleted >= levelAmountGoal)
            OnGameComplete?.Invoke(this, EventArgs.Empty);

        OnLevelComplete?.Invoke(this, EventArgs.Empty);
    }

    public string CalculateGrade() {
        int score = scoreSystem.Score;
        int grade = 0;
        string gradeStr;

        switch (score) {
            case < 750:
                grade = 0;
                break;
            case < 1000:
                grade = 1;
                break;
            case < 1250:
                grade = 2;
                break;
            case < 1500:
                grade = 3;
                break;
            case < 2250:
                grade = 4;
                break;
            case < 3000:
                grade = 5;
                break;
            case < 4000:
                grade = 6;
                break;
            default:
                grade = 7;
                break;
        }

        float elapsedTime = timer.ElapsedTime;
        int elapsedMinutes = (int)Mathf.Round(elapsedTime % 60);

        if (elapsedMinutes > 1 && elapsedMinutes < 3) grade--;
        else if (elapsedTime > 3 && elapsedMinutes < 5) grade -= 2;
        else grade -= 3;

        if (playerDeaths > 10) grade -= 2;
        if (playerDeaths > 5) grade--;

        switch (grade) {
            case <1:
                gradeStr = "F-";
                break;
            case 1:
                gradeStr = "F";
                break;
            case 2:
                gradeStr = "E";
                break;
            case 3:
                gradeStr = "D";
                break;
            case 4:
                gradeStr = "C";
                break;
            case 5:
                gradeStr = "B";
                break;
            case 6:
                gradeStr = "A";
                break;
            default:
                gradeStr = "S!";
                break;
        }

        return gradeStr;
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

    public int GoalLevelsAmount {
        get => levelAmountGoal;
    }

    public int CompletedLevelsAmount {
        get => levelsCompleted;
    }

    public int PlayerDeaths {
        get => playerDeaths;
    }

    public UIController UIController {
        get => uiController;
    }

    public ScoreSystem ScoreSystem {
        get => scoreSystem;
    }

    public AudioSource AudioPlayer {
        get => audioSource;
    }

    public Timer Timer {
        get => timer;
    }

    public Transform PlayerTransform {
        get => player.transform;
    }

    public MapGenerator MapGen {
        get => mapGenerator;
    }
}
