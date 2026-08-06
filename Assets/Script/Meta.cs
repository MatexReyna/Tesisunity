using UnityEngine;

public class Meta : MonoBehaviour
{
    public GameObject imagenGanaste;
    public GameObject imagenPerdiste;

    private bool terminado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("TOCO LA META: " + other.name);

        if (terminado)
            return;

        if (other.name == "Drake")
        {
            terminado = true;

            Debug.Log("GANO DRAKE");

            if (imagenGanaste != null)
                imagenGanaste.SetActive(true);

            Time.timeScale = 0f;
        }

        if (other.name == "luke")
        {
            terminado = true;

            Debug.Log("GANO LUKE");

            if (imagenPerdiste != null)
                imagenPerdiste.SetActive(true);

            Time.timeScale = 0f;
        }
    }
}