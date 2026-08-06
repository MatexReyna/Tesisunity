using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Marcadores")]
    public Image marcadorJugador;
    public Image marcadorBot;

    [Header("Sprites Jugador")]
    public Sprite[] spritesJugador;

    [Header("Sprites Bot")]
    public Sprite[] spritesBot;

    private int puntosJugador = 0;
    private int puntosBot = 0;

    void Start()
    {
        ActualizarMarcadores();
    }

    void Update()
    {
        // Solo para probar el marcador
        if (Input.GetKeyDown(KeyCode.P))
        {
            PuntoJugador();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            PuntoBot();
        }
    }

    public void PuntoJugador()
    {
        if (puntosJugador < 4)
        {
            puntosJugador++;
            ActualizarMarcadores();
        }
    }

    public void PuntoBot()
    {
        if (puntosBot < 4)
        {
            puntosBot++;
            ActualizarMarcadores();
        }
    }

    void ActualizarMarcadores()
    {
        marcadorJugador.sprite = spritesJugador[puntosJugador];
        marcadorBot.sprite = spritesBot[puntosBot];
    }
}