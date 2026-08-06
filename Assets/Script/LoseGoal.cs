using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Luke"))
        {
            SceneManager.LoadScene("Perdiste");
        }
    }
}
