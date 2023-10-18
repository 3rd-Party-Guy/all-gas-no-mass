using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    [SerializeField] private float velocityThreshhold;
    private void OnCollisionEnter2D(Collision2D other) {
        GameObject col = other.GetContact(0).collider.gameObject;
        if (!col.CompareTag("Player")) return;
        Debug.Log("COLLISION WITH " + col);
        float velocity = other.relativeVelocity.magnitude;
        if (velocity > velocityThreshhold) GameController.Instance.GameOver();
    }
}
