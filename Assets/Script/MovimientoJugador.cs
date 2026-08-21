using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 10f;

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
    }

    void Update()
    {
        // Movimiento
        float movimiento = Input.GetAxisRaw("Horizontal");

        rb.linearVelocity = new Vector2(
            movimiento * velocidad,
            rb.linearVelocity.y
        );

        // Animación de correr
        if (movimiento != 0)
        {
            animator.SetBool("Corriendo", true);
        }
        else
        {
            animator.SetBool("Corriendo", false);
        }

        // Detectar si está en el suelo
        enSuelo = Physics2D.OverlapCircle(
            detectorSuelo.position,
            radioSuelo,
            capaSuelo
        );

        // Actualizar animación de salto
        animator.SetBool("Saltando", !enSuelo);

        // Salto
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)) && enSuelo)
        {
            rb.linearVelocity = new Vector2(
                rb.linearVelocity.x,
                fuerzaSalto
            );

            // Activar animación de salto inmediatamente
            animator.SetBool("Saltando", true);
        }
    }
}