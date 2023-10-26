using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalTracer : MonoBehaviour
{
    [SerializeField] private float paddingX, paddingY;
    Vector3 screenPos;
    Vector2 onScreenPos;
    float max;
    GameObject curGoal;

    private void Update() {
        curGoal = GetGoal();
        CalculateTracker(curGoal);
    }

    private GameObject GetGoal() {
        GameObject[] landpoints = GameObject.FindGameObjectsWithTag("Landpoint");
        GameObject closest = null;

        float closestDistance = Mathf.Infinity;
        Vector3 curPos = GameController.Instance.PlayerTransform.position;
        
        foreach(GameObject goal in landpoints) {
            Vector3 goalPos = goal.transform.position;
            Vector3 dirToGoal = goalPos - curPos;
            float sqrDisToTarge = dirToGoal.magnitude;
        
            if (sqrDisToTarge < closestDistance) {
                closestDistance = sqrDisToTarge;
                closest = goal;
            }
        }

        return closest;
    }

    private void CalculateTracker(GameObject goal) {
        if (goal == null) {
            gameObject.GetComponent<Image>().enabled = false;
            Debug.LogWarning("No Goal found");
            return;
        }
        Vector3 toPos = goal.transform.position;
        Vector3 fromPos = Camera.main.transform.position;
        fromPos.z = 0f;
        Vector3 dir = (toPos - fromPos).normalized;
        float angle = CalculateVectorAngle(dir);
        
        transform.up = (toPos - GameController.Instance.PlayerTransform.position).normalized;

        Vector3 targetPosScreen = Camera.main.WorldToScreenPoint(toPos);
        bool isOffScreen = (targetPosScreen.x <= 0 || targetPosScreen.x >= Screen.width ||
                            targetPosScreen.y <= 0 || targetPosScreen.y >= Screen.height);
        gameObject.GetComponent<Image>().enabled = isOffScreen;
        
        if (isOffScreen) {
            Vector3 cappedTargetScreenPos = targetPosScreen;
            float xMin = paddingX;
            float xMax = Screen.width - paddingX;
            float yMin = paddingY;
            float yMax = Screen.height - paddingY;

            if (cappedTargetScreenPos.x < xMin) cappedTargetScreenPos.x = xMin;
            else if (cappedTargetScreenPos.x > xMax) cappedTargetScreenPos.x = xMax;
            if (cappedTargetScreenPos.y < yMin) cappedTargetScreenPos.y = yMin;
            else if (cappedTargetScreenPos.y > yMax) cappedTargetScreenPos.y = yMax;

            transform.position = cappedTargetScreenPos;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0f);
        }
    }

    private float CalculateVectorAngle(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }
}