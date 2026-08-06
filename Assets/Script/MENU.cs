using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Ir al nivel 1
    public void Jugar()
    {
        SceneManager.LoadScene("Nivel1");
    }

    // Ir a la escena de controles
    public void Controles()
    {
        SceneManager.LoadScene("Controles");
    }

    // Volver al menú principal (opcional para después)
    public void MenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }
}