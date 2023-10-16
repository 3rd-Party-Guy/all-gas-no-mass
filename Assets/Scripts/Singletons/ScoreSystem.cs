using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event EventHandler OnScoreChange;
    private int score;

    public int Score {
        get => score;
        set {
            score = value;
            OnScoreChange?.Invoke(this, EventArgs.Empty);
        }
    }
}
