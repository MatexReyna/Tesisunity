using UnityEngine;

public class Pelota : MonoBehaviour
{
    [Header("Física de la pelota")]
    public float fuerzaHorizontal = 5.5f;
    public float fuerzaVertical = 7f;
    public float velocidadMaxima = 10f;

    [Header("Sonido del golpe")]
    public AudioSource audioSource;
    public AudioClip sonidoGolpe;

    [Header("Sonido del silbato")]
    public AudioClip sonidoSilbato;

    private Rigidbody2D rb;
    private GameManager gameManager;

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

    void FixedUpdate()
    {
        // Evita que la pelota alcance velocidades exageradas
        if (rb.linearVelocity.magnitude > velocidadMaxima)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadMaxima;
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
            MoverPelota(collision);
        }

        // ==========================================
        // TOQUE DE LA RED
        // ==========================================

        else if (collision.gameObject.CompareTag("red"))
        {
            Debug.Log("🏐 La pelota tocó la red.");
        }
    }

    // ==========================================
    // SONIDO DEL GOLPE
    // ==========================================

    void ReproducirSonidoGolpe()
    {
        if (audioSource != null && sonidoGolpe != null)
        {
            audioSource.PlayOneShot(sonidoGolpe);
        }
    }

    // ==========================================
    // MOVIMIENTO DE LA PELOTA
    // ==========================================

    void MoverPelota(Collision2D collision)
    {
        float direccion;

        if (collision.transform.position.x < transform.position.x)
        {
            direccion = 1f;
        }
        else
        {
            direccion = -1f;
        }

        // Conservamos parte del movimiento actual
        float velocidadVertical = rb.linearVelocity.y;

        // Impulso controlado hacia el otro lado
        rb.linearVelocity = new Vector2(
            direccion * fuerzaHorizontal,
            velocidadVertical * 0.35f
        );

        rb.AddForce(
            new Vector2(0f, fuerzaVertical),
            ForceMode2D.Impulse
        );
    }
}