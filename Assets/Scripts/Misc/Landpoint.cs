using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landpoint : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    private void OnTriggerEnter2D(Collider2D col) {
        if (!col.gameObject.CompareTag("Player")) return;
        if (!GameController.Instance.ScoreSystem.IsEnough()) {
            GameController.Instance.UIController.FlashMessage("More Coins");
            return;
        }
        GameController.Instance.AudioPlayer.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
        GameController.Instance.CompleteLevel();
    }
}