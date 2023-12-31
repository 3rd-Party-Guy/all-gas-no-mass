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
        minScore = 0;


        GameController.Instance.OnLevelComplete += CalculateMinScore;
        CalculateMinScore(this, EventArgs.Empty);
    }

    private void AddLevelScore(object e, EventArgs data) {
        Score += 500;
    }
    
    private void CalculateMinScore(object e, EventArgs data) {
        int minScoreMin = minScore;
        int minScoreMax = (GameController.Instance.CompletedLevelsAmount + 3) * 250 + 
                            GameController.Instance.CompletedLevelsAmount * 500;

        minScore = UnityEngine.Random.Range(minScoreMin, minScoreMax);
        OnScoreChange?.Invoke(this, EventArgs.Empty);
    }

    public bool IsEnough() {
        return (score >= minScore);
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
