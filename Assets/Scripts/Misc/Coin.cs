using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int scoreAmount = 10;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            GameController.Instance.ScoreSystem.Score += scoreAmount;
            Destroy(gameObject);
        }
    }
}
