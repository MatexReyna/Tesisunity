using UnityEngine;

public class Pelota : MonoBehaviour
{
    [Header("Fuerza del contacto")]
    public float fuerzaHorizontal = 8f;
    public float fuerzaVertical = 6f;

    [Header("Sonido del golpe")]
    public AudioSource audioSource;
    public AudioClip sonidoGolpe;

    [Header("Sonido del silbato")]
    public AudioClip sonidoSilbato;

    private Rigidbody2D rb;
    private GameManager gameManager;

    // Contador de toques del equipo que tiene la pelota
    private int cantidadToques = 0;

    // Guarda quién hizo el último toque
    private string ultimoEquipo = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ==========================================
        // TOQUE DEL JUGADOR
        // ==========================================

        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.ToqueJugador();
            }

            ReproducirSonidoGolpe();

            ContarToque("Jugador");

            MoverPelota(collision);
        }

        // ==========================================
        // TOQUE DEL BOT
        // ==========================================

        else if (collision.gameObject.CompareTag("Bot"))
        {
            if (gameManager != null)
            {
                gameManager.ToqueBot();
            }

            ReproducirSonidoGolpe();

            ContarToque("Bot");

            MoverPelota(collision);
        }

        // ==========================================
        // TOQUE DE LA RED
        // ==========================================

        else if (collision.gameObject.CompareTag("red"))
        {
            ResetearToques();

            Debug.Log("🏐 La pelota tocó la red. Contador reiniciado.");
        }
    }

    void ContarToque(string equipo)
    {
        // Si cambia de equipo, empezar nuevamente desde 1
        if (ultimoEquipo != "" && ultimoEquipo != equipo)
        {
            cantidadToques = 0;
        }

        cantidadToques++;

        ultimoEquipo = equipo;

        Debug.Log("🏐 " + equipo + " tiene " + cantidadToques + " toque(s).");

        // Cuarto toque
        if (cantidadToques >= 4)
        {
            ReproducirSilbato();

            ResetearToques();
        }
    }

    void ResetearToques()
    {
        cantidadToques = 0;
        ultimoEquipo = "";
    }

    void ReproducirSonidoGolpe()
    {
        if (audioSource != null && sonidoGolpe != null)
        {
            audioSource.PlayOneShot(sonidoGolpe);
        }
    }

    void ReproducirSilbato()
    {
        if (audioSource != null && sonidoSilbato != null)
        {
            audioSource.PlayOneShot(sonidoSilbato);

            Debug.Log("📢 ¡SILBATO! ¡4 TOQUES!");
        }
    }

    void MoverPelota(Collision2D collision)
    {
        float direccion;

        if (collision.transform.position.x < transform.position.x)
            direccion = 1f;
        else
            direccion = -1f;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        rb.AddForce(
            new Vector2(direccion * fuerzaHorizontal, fuerzaVertical),
            ForceMode2D.Impulse
        );
    }
}