using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Landpoint : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Player")) {
            GameController.Instance.CompleteLevel();
        }
    }
}
