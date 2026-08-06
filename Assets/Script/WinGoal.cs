using UnityEngine;
using UnityEngine.SceneManagement;

public class TestMeta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TRIGGER FUNCIONA con: " + other.gameObject.name);

        SceneManager.LoadScene("Ganaste");
    }
}