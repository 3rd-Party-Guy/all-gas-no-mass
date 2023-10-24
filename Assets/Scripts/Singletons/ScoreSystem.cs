using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event EventHandler OnScoreChange;
    private int score;
    private int minScore;

    private void Start() {
        GameController.Instance.OnLevelComplete += CalculateMinScore;
    }

    private void CalculateMinScore(object e, EventArgs data) {
        int minScoreMin = GameController.Instance.CompletedLevelsAmount * 250;
        int minScoreMax = (GameController.Instance.CompletedLevelsAmount + 1) * 250;

        minScore = UnityEngine.Random.Range(minScoreMin, minScoreMax);
    }

    public int Score {
        get => score;
        set {
            score = value;
            OnScoreChange?.Invoke(this, EventArgs.Empty);
        }
    }

    public int MinScore {
        get => minScore;
    }
}
