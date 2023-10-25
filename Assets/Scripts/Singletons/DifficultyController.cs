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

    public void ManageDifficulty() => ManageDifficulty(this, EventArgs.Empty);

    public void ManageDifficulty(object e, EventArgs data) {
        Debug.Log("Difficulty Management Initiated...");
        curLevel = GameController.Instance.CompletedLevelsAmount + 1;
        ManageMovement();
        ManageMapSize();
    }


    private void ManageMovement() {
        Debug.Log("Player Movement Management Initiated...");

        Movement playerMov = GameController.Instance.PlayerTransform.GetComponent<Movement>();

        switch (curLevel) {
            case 1:
                playerMov.ChangeMovementDifficulty(650f, 600f, 1);
                break;
            case 2:
                playerMov.ChangeMovementDifficulty(650f, 600f, 0.7f);
                break;
            case 3:
                playerMov.ChangeMovementDifficulty(725f, 650f, 0.7f);
                break;
            case 4:
                playerMov.ChangeMovementDifficulty(750f, 650f, 0.7f);
                break;
            case 5:
                playerMov.ChangeMovementDifficulty(750f, 700f, 0.7f);
                break;
            case 6:
                playerMov.ChangeMovementDifficulty(750f, 700f, 0.5f);
                break;
            case 7:
                playerMov.ChangeMovementDifficulty(750f, 750f, 0.4f);
                break;
            case 8:
                playerMov.ChangeMovementDifficulty(800f, 800f, 0.3f);
                break;
            case 9:
                playerMov.ChangeMovementDifficulty(900f, 900f, 0.2f);
                break;
            case 10:
                playerMov.ChangeMovementDifficulty(1000f, 1000f, 0.15f);
                break;
            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }

    private void ManageMapSize() {
        Debug.Log("Map Size Management Initiated...");

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
                mapGen.RandomFillPercent = 55;
                break;
            case 6:
                mapGen.ChangeSize(420, 640);
                mapGen.RandomFillPercent = 55;
                break;
            case 7:
                mapGen.ChangeSize(1000, 1000);
                mapGen.RandomFillPercent = 75;
                break;
            case 8:
                mapGen.ChangeSize(1000, 1000);
                mapGen.RandomFillPercent = 75;
                break;
            case 9:
                mapGen.ChangeSize(1000, 1000);
                mapGen.RandomFillPercent = 30;
                break;
            case 10:
                mapGen.ChangeSize(1500, 1500);
                mapGen.RandomFillPercent = 25;
                break;
            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }
}
