using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    [Header("Target Configuration")]
    [SerializeField] Transform target;

    [Space]

    [Header("Camera Movement Configuration")]
    [SerializeField] float dampTime = 10f;
    [SerializeField] float xOffset, yOffset = 0f;

    [Space]

    [Header("Camera Reaction to Player Movement")]
    [SerializeField] bool movementBasedOffset = false;
    [SerializeField] float movementOffsetMultiplier = 2f;
    [SerializeField] float movementDampTime = 1f;
    float xMovementOffset, yMovementOffset = 0f;
    float xMovementOffsetVelocity, yMovementOffsetVelocity = 0f;

    public void FixedUpdate() {
        if (target == null) return;
        HandleSmoothFollow();
        HandleMovementBasedOffset();
    }

    public void PointAt(Transform newTarget, float secAmount = 1f) => PointAtCoroutine(newTarget, secAmount);
    
    private void HandleSmoothFollow() {
        float targetX = Mathf.Lerp(transform.position.x, target.position.x + xOffset + xMovementOffset, 1/dampTime * Time.deltaTime);
        float targetY = Mathf.Lerp(transform.position.y, target.position.y + yOffset + yMovementOffset, 1/dampTime * Time.deltaTime);
        
        transform.position = new Vector3(targetX, targetY, transform.position.z);
    }

    private void HandleMovementBasedOffset() {
        if (target.gameObject.CompareTag("Player") && movementBasedOffset) {
            Vector2 playerMovementVec = target.GetComponent<Movement>().GetMovementVector();
            xMovementOffset = Mathf.SmoothDamp(xMovementOffset, playerMovementVec.x * movementOffsetMultiplier, ref xMovementOffsetVelocity, movementDampTime);
            yMovementOffset = Mathf.SmoothDamp(yMovementOffset, playerMovementVec.y * movementOffsetMultiplier, ref yMovementOffsetVelocity, movementDampTime);
        }
    }

    private IEnumerator PointAtCoroutine(Transform newTarget, float secAmount) {
        Transform oldTarget = target;
        target = newTarget;
        yield return new WaitForSeconds(secAmount);
        target = oldTarget;
    }

    public Transform Target {
        get => target;
        set => target = value;
    }
}
