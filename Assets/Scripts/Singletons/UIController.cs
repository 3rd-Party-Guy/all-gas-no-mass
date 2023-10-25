using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Timer))]
public class UIController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text levelsText;
    [SerializeField] TMP_Text updateText;
    [SerializeField] TMP_Text timeText;

    [SerializeField] GameObject gameFinishedObject;
    [SerializeField] TMP_Text finalScoreText;
    [SerializeField] TMP_Text finalGradeText;


    [Space]

    [Header("Parameters")]
    [Tooltip("In seconds")]
    [SerializeField] const float defaultFlashTime = 1f;
    Timer timer;

    private void Start() {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainUI"));
        updateText.gameObject.SetActive(false);

        timer = GetComponent<Timer>();

        GameController.Instance.ScoreSystem.OnScoreChange += UpdateScoreUI;
        GameController.Instance.OnPlayerDeath  += FlashDeath;
        GameController.Instance.OnLevelComplete += CompleteLevelUI;
        GameController.Instance.OnLevelComplete += UpdateLevelScoreUI;
        GameController.Instance.OnGameComplete += OnGameCompleted;

        gameFinishedObject.SetActive(false);
    }

    private void Update() {
        if (timer.ElapsedTime > TimeSpan.MaxValue.TotalSeconds) {
            timeText.text = "Get help.";
            return;
        }

        TimeSpan elapsedTime = TimeSpan.FromSeconds(timer.ElapsedTime);
        
        if (elapsedTime.TotalMinutes < 1)
            timeText.text = elapsedTime.ToString(@"ss\:ff");
        else
            timeText.text = elapsedTime.ToString(@"mm\:ss\:ff");
    }

    private void OnGameCompleted(object e, EventArgs data) {
        gameFinishedObject.SetActive(true);

        finalScoreText.text += " " + GameController.Instance.ScoreSystem.Score.ToString();
        finalGradeText.text += " " + GameController.Instance.CalculateGrade();
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
        levelsText.text = newLevelScore.ToString();
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
}
