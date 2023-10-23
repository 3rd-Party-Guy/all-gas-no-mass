using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int scoreAmount = 10;
    [SerializeField] AudioClip[] scoreAudios;
    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.CompareTag("Player")) {
            GameController.Instance.ScoreSystem.Score += scoreAmount;
            GameController.Instance.AudioPlayer.PlayOneShot(scoreAudios[Random.Range(0, scoreAudios.Length - 1)]);
            Destroy(gameObject);
        }
    }
}
