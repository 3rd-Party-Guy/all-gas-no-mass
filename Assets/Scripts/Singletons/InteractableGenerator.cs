using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject goalPrefab;
    
    [Space]
    
    [Header("Interactables Amount")]
    [SerializeField] private uint coinAmount;
    [SerializeField] private uint goalAmount = 1;

    private void Start() {
        GameController.Instance.MapGen.OnLevelGenerationComplete += GenerateInteractables;
    }

    public void GenerateInteractables(object e, EventArgs data) {
        GenerateCoins();
        GenerateGoals();
    }

    private void GenerateCoins() {
        for (uint i = 0; i < coinAmount; i++) {
            GameObject coinObj = Instantiate(coinPrefab);
            Vector3 coinPos = GameController.Instance.GetFreePosition();
            coinObj.transform.position = coinPos;
        }
    }

    private void GenerateGoals() {
        for (uint i = 0; i < goalAmount; i++) {
            GameObject goalObj = Instantiate(goalPrefab);
            Vector3 goalPos = GameController.Instance.GetFreePosition();
            goalObj.transform.position = goalPos;
        }
    }
}
