using UnityEngine;

public class ZonaPunto : MonoBehaviour
{
    public bool ladoJugador;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pelota"))
            return;

        if (ladoJugador)
        {
            gameManager.PuntoBot();
        }
        else
        {
            gameManager.PuntoJugador();
        }
    }
}
