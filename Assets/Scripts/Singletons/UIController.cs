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
    [SerializeField] TMP_Text gameStateUpdateText;
    [SerializeField] TMP_Text timeText;


    [Space]

    [Header("Parameters")]
    [Tooltip("In seconds")]
    [SerializeField] const float defaultFlashTime = 1f;
    Timer timer;

    private void Start() {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainUI"));
        gameStateUpdateText.gameObject.SetActive(false);

        timer = GetComponent<Timer>();

        GameController.Instance.ScoreSystem.OnScoreChange += UpdateScoreUI;
        GameController.Instance.OnGameStateChange  += UpdateGameState;
        GameController.Instance.OnLevelComplete += CompleteLevelUI;
        GameController.Instance.OnLevelComplete += UpdateLevelScoreUI;
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
        // timeText.text = timer.ElapsedTime.ToString("0.00", CultureInfo.InvariantCulture);
    }

    private void UpdateGameState(object e, EventArgs args) {
        GameStateType curGameState = GameController.Instance.GameState;

        switch (curGameState) {
        case GameStateType.GameOver:
            StartCoroutine(FlashStatusChange("Try Again"));
            break;
        }
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
        scoreText.text = newScore.ToString();
    }

    private IEnumerator FlashStatusChange(string statusText, float flashTime = defaultFlashTime) {
        gameStateUpdateText.gameObject.SetActive(true);
        gameStateUpdateText.text = statusText;
        yield return new WaitForSeconds(flashTime);
        gameStateUpdateText.gameObject.SetActive(false);
    }
}
