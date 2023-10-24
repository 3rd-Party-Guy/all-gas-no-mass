using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float time = 0f;
    bool isTimerOn = false;

    private void Start() {
        isTimerOn = true;
        GameController.Instance.OnGameCompleted += StopTimer;
    }

    private void Update() {
        if (isTimerOn == false) return;
        ElapsedTime += Time.deltaTime;
    }

    private void StopTimer(object e, EventArgs args) => StopTimer();

    public void StopTimer() {
        isTimerOn = false;
    }

    public float ElapsedTime {
        get => time;
        set => time = value;
    }
}
