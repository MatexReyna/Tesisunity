using UnityEngine;

public class IntroduccionNivel2 : MonoBehaviour
{
    [Header("Sprites de la introducción")]
    public SpriteRenderer dialogo;

    public Sprite[] spritesDialogo;

    [Header("Objetos del nivel")]
    public GameObject jugador;
    public GameObject bot;
    public Rigidbody2D pelota;

    private int dialogoActual = 0;
    private bool introduccionTerminada = false;

    void Start()
    {
        // Pausar completamente el juego
        Time.timeScale = 0f;

        // Mostrar el primer diálogo
        ActualizarDialogo();

        Debug.Log("Introducción del Nivel 2 iniciada. Presiona F para continuar.");
    }

    void Update()
    {
        // Si ya terminó la introducción, no hacemos nada
        if (introduccionTerminada)
            return;

        // Avanzar con F
        if (Input.GetKeyDown(KeyCode.F))
        {
            SiguienteDialogo();
        }
    }

    void ActualizarDialogo()
    {
        if (spritesDialogo != null &&
            spritesDialogo.Length > dialogoActual)
        {
            dialogo.sprite = spritesDialogo[dialogoActual];
        }
    }

    void SiguienteDialogo()
    {
        dialogoActual++;

        // Si ya pasamos el último diálogo
        if (dialogoActual >= spritesDialogo.Length)
        {
            IniciarNivel();
            return;
        }

        // Mostrar siguiente sprite
        ActualizarDialogo();
    }

    void IniciarNivel()
    {
        introduccionTerminada = true;

        // Ocultar el diálogo
        dialogo.enabled = false;

        // Reanudar el juego
        Time.timeScale = 1f;

        Debug.Log("🏐 ¡Comienza el Nivel 2!");
    }
}
