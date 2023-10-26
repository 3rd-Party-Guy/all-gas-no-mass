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
        curLevel = GameController.Instance.CompletedLevelsAmount + 1;
        ManageMovement();
        ManageMapSize();
    }


    private void ManageMovement() {
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
                playerMov.ChangeMovementDifficulty(1000f, 900f, 0.5f);
                break;
            case 6:
                playerMov.ChangeMovementDifficulty(1000f, 100f, 0.5f);
                break;
            case 7:
                playerMov.ChangeMovementDifficulty(1200f, 1200f, 0.4f);
                break;
            case 8:
                playerMov.ChangeMovementDifficulty(1200f, 1200f, 0.3f);
                break;
            case 9:
                playerMov.ChangeMovementDifficulty(1200f, 1200f, 0.2f);
                break;
            case 10:
                playerMov.ChangeMovementDifficulty(1250f, 1250f, 0.15f);
                break;
            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }

    private void ManageMapSize() {
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
                mapGen.ChangeSize(240, 320);
                mapGen.RandomFillPercent = 40;
                break;
            case 6:
                mapGen.ChangeSize(320, 320);
                mapGen.RandomFillPercent = 40;
                break;
            case 7:
                mapGen.ChangeSize(480, 360);
                mapGen.RandomFillPercent = 45;
                break;
            case 8:
                mapGen.ChangeSize(480, 480);
                mapGen.RandomFillPercent = 48;
                break;
            case 9:
                mapGen.RandomFillPercent = 30;
                break;
            case 10:
                mapGen.RandomFillPercent = 50;
                break;
            default:
                Debug.LogError("No Difficulty for this Level");
                break;
        }
    }
}
