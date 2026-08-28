using UnityEngine;

public class BotIA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pelota;
    public Transform detectorSuelo;

    [Header("Movimiento")]
    public float velocidad = 4f;
    public float fuerzaSalto = 9f;

    [Header("Límites")]
    public float limiteIzquierdo = -329.5f;
    public float limiteDerecho = -316.8f;

    [Header("Espera")]
    public float posicionEspera = -324f;

    [Header("Configuración")]
    public float zonaMuerta = 0.8f;

    [Header("Suelo")]
    public LayerMask capaSuelo;
    public float radioSuelo = 0.2f;

    [Header("Salto")]
    public float tiempoEntreSaltos = 0.5f;

    // ==========================================
    // SONIDO
    // ==========================================

    [Header("Sonido Pasos")]
    public AudioSource audioSource;
    public AudioClip sonidoPasos;

    private float siguienteSalto = 0f;
    private Rigidbody2D rb;

    // ANIMACIÓN
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // ANIMACIÓN
        animator = GetComponent<Animator>();

        // SONIDO
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = 1f;
        }
    }

    void Update()
    {
        if (pelota == null)
            return;

        bool enSuelo = Physics2D.OverlapCircle(
            detectorSuelo.position,
            radioSuelo,
            capaSuelo);

        float destino;

        // Si la pelota está del lado del jugador,
        // el bot vuelve al centro de su cancha.
        if (pelota.position.x < -329.89f)
        {
            destino = posicionEspera;
        }
        else
        {
            destino = pelota.position.x;
        }

        destino = Mathf.Clamp(destino, limiteIzquierdo, limiteDerecho);

        float distancia = Mathf.Abs(destino - transform.position.x);

        // ==========================================
        // MOVIMIENTO
        // ==========================================

        if (distancia > zonaMuerta)
        {
            float direccion = Mathf.Sign(destino - transform.position.x);

            rb.linearVelocity = new Vector2(
                direccion * velocidad,
                rb.linearVelocity.y);

            // ==========================================
            // SONIDO DE PASOS DE ADA
            // ==========================================

            if (audioSource != null && sonidoPasos != null)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = sonidoPasos;
                    audioSource.loop = true;
                    audioSource.Play();

                    Debug.Log("SONIDO DE PASOS DE ADA REPRODUCIÉNDOSE");
                }
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(
                0,
                rb.linearVelocity.y);

            // DETENER SONIDO
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();

                Debug.Log("SONIDO DE PASOS DE ADA DETENIDO");
            }
        }

        // ==========================================
        // SALTO CON COOLDOWN
        // ==========================================

        if (enSuelo &&
            Time.time >= siguienteSalto &&
            pelota.position.y > transform.position.y + 0.5f &&
            Mathf.Abs(pelota.position.x - transform.position.x) < 1.2f)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto);

            siguienteSalto = Time.time + tiempoEntreSaltos;
        }

        // ==========================================
        // ANIMACIONES DE ADA
        // ==========================================

        if (animator != null)
        {
            // SALTAR
            if (!enSuelo)
            {
                animator.SetBool("Saltar", true);
                animator.SetBool("Correr", false);
            }
            else
            {
                animator.SetBool("Saltar", false);

                // CORRER
                if (distancia > zonaMuerta)
                {
                    animator.SetBool("Correr", true);
                }
                // QUIETA
                else
                {
                    animator.SetBool("Correr", false);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (detectorSuelo != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(
                detectorSuelo.position,
                radioSuelo);
        }
    }
}