using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

    [Header("SONIDO")]
    public AudioSource audioSource;
    public AudioClip sonidoPasos;

    private Rigidbody2D rb;
    private Animator animator;

    public Transform detectorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;

    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Si no asignaste el AudioSource manualmente,
        // busca uno en el mismo objeto.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Configuración del sonido
        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }
    }

    void Update()
    {
        // =========================
        // MOVIMIENTO
        // =========================

        float movimiento = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        // =========================
        // ANIMACIÓN Y SONIDO
        // =========================

        if (movimiento != 0)
        {
            animator.SetBool("Corriendo", true);

            // SONIDO DE PASOS
            if (audioSource != null && sonidoPasos != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = sonidoPasos;
                    audioSource.loop = true;
                    audioSource.Play();

                    Debug.Log("SONIDO DE PASOS REPRODUCIÉNDOSE");
                }
            }
            else
            {
                Debug.LogWarning("Falta asignar AudioSource o SonidoPasos");
            }
        }
        else
        {
            animator.SetBool("Corriendo", false);

            // DETENER SONIDO
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                Debug.Log("SONIDO DE PASOS DETENIDO");
            }
        }

        // =========================
        // DETECTAR SUELO
        // =========================

        enSuelo = Physics2D.OverlapCircle(
            detectorSuelo.position,
            radioSuelo,
            capaSuelo
        );

        animator.SetBool("Saltando", !enSuelo);

        // =========================
        // SALTO
        // =========================

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            animator.SetBool("Saltando", true);
        }
    }
}