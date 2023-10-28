using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

[RequireComponent(typeof(Timer))]
public class UIController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelsText;
    [SerializeField] TMP_Text updateText;
    [SerializeField] TMP_Text timeText;

    [SerializeField] GameObject gameFinishedUI;
    [SerializeField] GameObject pauseUI;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text finalTimeText;
    [SerializeField] TMP_Text finalDeathsText;
    [SerializeField] TMP_Text finalGradeText;

    [Header("UI Preferences")]
    [SerializeField] float timeCharWidth;

    [Space]

    [Header("Interactability")]
    [SerializeField] InputAction pauseAction;

    Timer timer;
    const float defaultFlashTime = 1f;
    bool isPaused;

    private void Start() {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainUI"));
        updateText.gameObject.SetActive(false);

        timer = GetComponent<Timer>();
        isPaused = false;

        GameController.Instance.ScoreSystem.OnScoreChange += UpdateScoreUI;
        GameController.Instance.OnPlayerDeath  += FlashDeath;
        GameController.Instance.OnLevelComplete += CompleteLevelUI;
        GameController.Instance.OnLevelComplete += UpdateLevelScoreUI;
        GameController.Instance.OnGameComplete += OnGameCompleted;

        gameFinishedUI.SetActive(false);

        pauseAction.started += ctx => Pause();
    }

    private void Update() {
        if (timer.ElapsedTime > TimeSpan.MaxValue.TotalSeconds) {
            timeText.text = "Get help.";
            return;
        }

        string textStr = FormatElapsedTimeUI();
        timeText.SetText($"<mspace={timeCharWidth}em>{textStr}");
    }

    private void Pause() {
        if (isPaused) {
            pauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
        else {
            pauseUI.SetActive(true);
            Time.timeScale = 0f;
        }

        isPaused = !isPaused;
        Cursor.visible = isPaused;
    }

    private void OnGameCompleted(object e, EventArgs data) {
        gameFinishedUI.SetActive(true);
        Cursor.visible = true;

        finalScoreText.text += " " + GameController.Instance.ScoreSystem.Score.ToString();
        finalGradeText.text += " " + GameController.Instance.CalculateGrade();
        finalTimeText.text += " " + FormatElapsedTimeUI();
        finalDeathsText.text += " " + GameController.Instance.PlayerDeaths;
    }

    public string FormatElapsedTimeUI() {
        TimeSpan elapsedTime = TimeSpan.FromSeconds(timer.ElapsedTime);
        string textString;

        if (elapsedTime.TotalMinutes < 1)
            textString = elapsedTime.ToString(@"ss\:ff");
        else
            textString = elapsedTime.ToString(@"mm\:ss\:ff");

        return textString;
    }

    public void FlashMessage(string msg) => StartCoroutine(FlashStatusChange(msg));

    private void FlashDeath(object e, EventArgs args) {
        StartCoroutine(FlashStatusChange("Death " + GameController.Instance.PlayerDeaths.ToString()));
    }

    private void CompleteLevelUI(object e, EventArgs args) {
        StartCoroutine(FlashStatusChange("Level Completed!"));
    }

    private void UpdateLevelScoreUI(object e, EventArgs args) {
        int newLevelScore = GameController.Instance.CompletedLevelsAmount;
        levelsText.text = newLevelScore.ToString() + "/" + GameController.Instance.GoalLevelsAmount;
    }

    private void UpdateScoreUI(object e, EventArgs args) {
        int newScore = GameController.Instance.ScoreSystem.Score;
        int newMinScore = GameController.Instance.ScoreSystem.MinScore;

        scoreText.text = newScore.ToString() + "/" + newMinScore.ToString();

        if (GameController.Instance.ScoreSystem.IsEnough())
            scoreText.color = new Color(0, 255, 0, 255);
        else
            scoreText.color = new Color(255, 0, 0, 255);
    }

    private IEnumerator FlashStatusChange(string statusText, float flashTime = defaultFlashTime) {
        updateText.gameObject.SetActive(true);
        updateText.text = statusText;
        yield return new WaitForSeconds(flashTime);
        updateText.gameObject.SetActive(false);
    }

    public void PlayAgain() {
        Cursor.visible = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameController.Instance.gameObject.SetActive(false);
        GameController.Instance.gameObject.SetActive(true);
        Destroy(gameFinishedUI.transform.parent.gameObject);
    }

    public void Exit() => Application.Quit();

    private void OnEnable() {
        pauseAction.Enable();
    }

    private void OnDisable() {
        pauseAction.Disable();
    }
}
