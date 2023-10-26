using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishedGameUIController : MonoBehaviour
{
    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameController.Instance.gameObject.SetActive(false);
        GameController.Instance.gameObject.SetActive(true);
        Destroy(transform.parent.gameObject);
    }

    public void Exit() => Application.Quit();
}
