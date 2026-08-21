using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Pelota")]
    public Rigidbody2D pelota;

    [Header("Saques")]
    public Transform saqueJugador;
    public Transform saqueBot;

    [Header("Jugadores")]
    public Transform jugador;
    public Transform bot;

    [Header("Posiciones iniciales")]
    public Transform inicioJugador;
    public Transform inicioBot;

    [Header("Tiempo de respawn")]
    public float tiempoRespawn = 1f;

    [Header("Regla de los 3 toques")]
    public int maximoToques = 3;

    [Header("Red")]
    public float posicionRed = -329.89f;

    [Header("Marcador del jugador")]
    public SpriteRenderer marcadorJugador;
    public Sprite[] spritesMarcadorJugador;

    private int puntosJugador = 0;

    [Header("Marcador del bot")]
    public SpriteRenderer marcadorBot;
    public Sprite[] spritesMarcadorBot;

    private int puntosBot = 0;

    private int toquesJugador = 0;
    private int toquesBot = 0;

    private bool pelotaEnLadoJugador;
    private bool puntoEnProceso = false;

    void Start()
    {
        // Determinamos inicialmente de qué lado está la pelota
        pelotaEnLadoJugador = pelota.transform.position.x < posicionRed;

        // Comenzamos mostrando 00
        ActualizarMarcadorJugador();
        ActualizarMarcadorBot();
    }

    void Update()
    {
        if (puntoEnProceso)
            return;

        ActualizarLadoPelota();
    }

    // ==========================================
    // DETECTAR CUANDO LA PELOTA CAMBIA DE LADO
    // ==========================================

    void ActualizarLadoPelota()
    {
        bool ladoActual = pelota.transform.position.x < posicionRed;

        if (ladoActual != pelotaEnLadoJugador)
        {
            pelotaEnLadoJugador = ladoActual;

            if (pelotaEnLadoJugador)
            {
                toquesJugador = 0;

                Debug.Log("La pelota pasó al jugador. Toques reiniciados.");
            }
            else
            {
                toquesBot = 0;

                Debug.Log("La pelota pasó al bot. Toques reiniciados.");
            }
        }
    }

    // ==========================================
    // TOQUE DEL JUGADOR
    // ==========================================

    public void ToqueJugador()
    {
        if (puntoEnProceso)
            return;

        toquesJugador++;

        Debug.Log("Toques del jugador: " + toquesJugador);

        if (toquesJugador > maximoToques)
        {
            Debug.Log("¡FALTA! El jugador hizo 4 toques.");

            PuntoBot();
        }
    }

    // ==========================================
    // TOQUE DEL BOT
    // ==========================================

    public void ToqueBot()
    {
        if (puntoEnProceso)
            return;

        toquesBot++;

        Debug.Log("Toques del bot: " + toquesBot);

        if (toquesBot > maximoToques)
        {
            Debug.Log("¡FALTA! El bot hizo 4 toques.");

            PuntoJugador();
        }
    }

    // ==========================================
    // PUNTO PARA EL JUGADOR
    // ==========================================

    public void PuntoJugador()
    {
        if (puntoEnProceso)
            return;

        puntoEnProceso = true;

        // Sumar punto al jugador
        if (puntosJugador < 4)
        {
            puntosJugador++;

            Debug.Log("Punto para el jugador: " + puntosJugador);

            ActualizarMarcadorJugador();

            // ==========================================
            // SI EL JUGADOR LLEGA A 04, GANA
            // ==========================================

            if (puntosJugador >= 4)
            {
                Debug.Log("🏆 ¡EL JUGADOR GANÓ!");

                SceneManager.LoadScene("GanasteNv2");

                return;
            }
        }

        ReiniciarToques();

        StartCoroutine(Respawn(saqueJugador.position));
    }

    // ==========================================
    // ACTUALIZAR MARCADOR DEL JUGADOR
    // ==========================================

    void ActualizarMarcadorJugador()
    {
        if (marcadorJugador != null &&
            spritesMarcadorJugador != null &&
            spritesMarcadorJugador.Length > puntosJugador)
        {
            marcadorJugador.sprite = spritesMarcadorJugador[puntosJugador];
        }
    }

    // ==========================================
    // PUNTO PARA EL BOT
    // ==========================================

    public void PuntoBot()
    {
        if (puntoEnProceso)
            return;

        puntoEnProceso = true;

        // Sumar punto al bot
        if (puntosBot < 4)
        {
            puntosBot++;

            Debug.Log("Punto para el bot: " + puntosBot);

            ActualizarMarcadorBot();

            // ==========================================
            // SI EL BOT LLEGA A 04, PIERDE EL JUGADOR
            // ==========================================

            if (puntosBot >= 4)
            {
                Debug.Log("❌ ¡EL BOT GANÓ! El jugador perdió.");

                SceneManager.LoadScene("PerdisteNv2");

                return;
            }
        }

        ReiniciarToques();

        StartCoroutine(Respawn(saqueBot.position));
    }

    // ==========================================
    // ACTUALIZAR MARCADOR DEL BOT
    // ==========================================

    void ActualizarMarcadorBot()
    {
        if (marcadorBot != null &&
            spritesMarcadorBot != null &&
            spritesMarcadorBot.Length > puntosBot)
        {
            marcadorBot.sprite = spritesMarcadorBot[puntosBot];
        }
    }

    // ==========================================
    // REINICIAR TOQUES
    // ==========================================

    void ReiniciarToques()
    {
        toquesJugador = 0;
        toquesBot = 0;
    }

    // ==========================================
    // RESPAWN
    // ==========================================

    IEnumerator Respawn(Vector3 posicionPelota)
    {
        pelota.linearVelocity = Vector2.zero;
        pelota.angularVelocity = 0f;
        pelota.gravityScale = 0f;

        jugador.position = inicioJugador.position;
        bot.position = inicioBot.position;

        pelota.transform.position = posicionPelota;

        // Actualizamos el lado de la pelota después del respawn
        pelotaEnLadoJugador = pelota.transform.position.x < posicionRed;

        yield return new WaitForSeconds(tiempoRespawn);

        pelota.gravityScale = 0.4f;

        puntoEnProceso = false;
    }
}