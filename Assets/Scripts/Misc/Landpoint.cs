using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landpoint : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            GameController.Instance.AudioPlayer.PlayOneShot(audioClips[Random.Range(0, audioClips.Length - 1)]);
            GameController.Instance.CompleteLevel();
        }
    }
}
