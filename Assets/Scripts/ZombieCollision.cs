using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ZombieCollision : MonoBehaviour
{
    public GameObject gameOverText;
    private bool hasCollided = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("Player"))
        {
            TriggerGameOver();
        }
    }

    public void TriggerGameOver()
    {
        if (hasCollided)
        {
            return;
        }

        hasCollided = true;
        Time.timeScale = 0f;
        gameOverText.SetActive(true);
        StartCoroutine(RestartAfterDelay());
    }

    IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(10f);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
