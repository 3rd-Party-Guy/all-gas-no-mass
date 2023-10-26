using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    int curLevel = 1;
    private void Start() {
        curLevel = 1;
    }

    public void ManageMovement() {
        curLevel = GameController.Instance.CompletedLevelsAmount + 1;
        Movement playerMov = GameController.Instance.PlayerTransform.GetComponent<Movement>();

        switch (curLevel) {
            case 1:
                playerMov.ChangeMovementDifficulty(650f, 800f, 1);
                break;
            case 2:
                playerMov.ChangeMovementDifficulty(850f, 900f, 0.5f);
                break;
            case 3:
                playerMov.ChangeMovementDifficulty(900f, 1000f, 0.3f);
                break;
            case 4:
                playerMov.ChangeMovementDifficulty(900f, 1100f, 0.3f);
                break;
            case 5:
                playerMov.ChangeMovementDifficulty(1100f, 1200f, 0.25f);
                break;

            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }

    public void ManageMapSize() {
        curLevel = GameController.Instance.CompletedLevelsAmount + 1;
        MapGenerator mapGen = GameController.Instance.MapGen;

        switch (curLevel) {
            case 1:
                mapGen.ChangeSize(360, 120);
                mapGen.RandomFillPercent = 40;
                break;
            case 2:
                mapGen.ChangeSize(100, 720);
                mapGen.RandomFillPercent = 40;
                break;
            case 3:
                mapGen.ChangeSize(720, 25);
                mapGen.RandomFillPercent = 30;
                break;
            case 4:
                mapGen.ChangeSize(25, 720);
                mapGen.RandomFillPercent = 33;
                break;
            case 5:
                mapGen.ChangeSize(500, 500);
                mapGen.RandomFillPercent = 40;
                break;
            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }
}
