using UnityEngine;

public class PelotaJugador3 : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    public Transform puntoAgarre;

    [Header("Agarrar pelota")]
    public float distanciaParaAgarrar = 1.5f;

    [Header("Lanzamiento")]
    public float velocidadLanzamiento = 6f;

    [Header("Animación")]
    public Animator animator;

    private Rigidbody2D rb;

    private bool agarrada = false;

    private Vector2 ultimaDireccion = Vector2.right;

    private bool animacionReproduciendose = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Si no asignamos el Animator manualmente,
        // lo buscamos automáticamente.
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        DetenerAnimacion();
    }

    void Update()
    {
        // Recordamos la última dirección en la que se movió Drake
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direccionMovimiento = new Vector2(horizontal, vertical);

        if (direccionMovimiento != Vector2.zero)
        {
            ultimaDireccion = direccionMovimiento.normalized;
        }

        // Presionar E
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!agarrada)
            {
                IntentarAgarrar();
            }
            else
            {
                Lanzar();
            }
        }

        // Si está agarrada, sigue la mano
        if (agarrada)
        {
            transform.position = puntoAgarre.position;

            DetenerAnimacion();
        }
        else
        {
            // Si la pelota se está moviendo,
            // reproducimos la animación.
            if (rb.linearVelocity.magnitude > 0.1f)
            {
                ReproducirAnimacion();
            }
            else
            {
                DetenerAnimacion();
            }
        }
    }

    void IntentarAgarrar()
    {
        if (jugador == null)
            return;

        float distancia = Vector2.Distance(
            transform.position,
            jugador.position
        );

        if (distancia <= distanciaParaAgarrar)
        {
            Agarrar();
        }
    }

    void Agarrar()
    {
        agarrada = true;

        rb.linearVelocity = Vector2.zero;

        rb.simulated = false;

        transform.SetParent(puntoAgarre);

        transform.localPosition = Vector3.zero;

        DetenerAnimacion();
    }

    void Lanzar()
    {
        agarrada = false;

        transform.SetParent(null);

        rb.simulated = true;

        rb.linearVelocity =
            ultimaDireccion * velocidadLanzamiento;

        // Al lanzarla comienza la animación
        ReproducirAnimacion();
    }

    void ReproducirAnimacion()
    {
        if (animator == null)
            return;

        if (!animacionReproduciendose)
        {
            animator.Play("PelotaAnim3");

            animacionReproduciendose = true;
        }

        animator.speed = 1f;
    }

    void DetenerAnimacion()
    {
        if (animator == null)
            return;

        animator.speed = 0f;

        animacionReproduciendose = false;
    }
}