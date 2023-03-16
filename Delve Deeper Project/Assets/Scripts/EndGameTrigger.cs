using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Game Over");
        }
    }
}
