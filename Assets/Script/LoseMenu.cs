using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseMenu : MonoBehaviour
{
    public void Reintentar()
    {
        SceneManager.LoadScene("Nivel1");
    }
}
