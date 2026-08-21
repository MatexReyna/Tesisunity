using UnityEngine;
using UnityEngine.SceneManagement;

public class PerdisteButtons : MonoBehaviour
{
    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void IntentarDeNuevo()
    {
        SceneManager.LoadScene("Nivel2");
    }
}