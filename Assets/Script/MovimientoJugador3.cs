using UnityEngine;

public class MovimientoJugador3 : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    [Header("Agacharse")]
    public float velocidadAgachado = 2.5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;

    private bool agachado;

    private Vector3 escalaNormal;
    private Vector3 escalaAgachado;

    // Animator
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Buscamos el Animator de Drake3
        animator = GetComponent<Animator>();

        // Guardamos el tamaño original
        escalaNormal = transform.localScale;

        // Tamaño cuando está agachado
        escalaAgachado = new Vector3(
            escalaNormal.x,
            escalaNormal.y * 0.6f,
            escalaNormal.z
        );
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(horizontal, vertical).normalized;

        // Mantener C para agacharse
        agachado = Input.GetKey(KeyCode.C);

        if (agachado)
        {
            transform.localScale = escalaAgachado;
        }
        else
        {
            transform.localScale = escalaNormal;
        }

        // Animaciones
        ActualizarAnimacion();
    }

    void FixedUpdate()
    {
        float velocidadActual = agachado ? velocidadAgachado : velocidad;

        rb.linearVelocity = movimiento * velocidadActual;
    }

    void ActualizarAnimacion()
    {
        if (animator == null)
            return;

        // Si está agachado
        if (agachado)
        {
            animator.Play("Crouch3");
        }
        // Si se está moviendo
        else if (movimiento != Vector2.zero)
        {
            animator.Play("Run3");
        }
        // Si está quieto
        else
        {
            animator.Play("State3");
        }
    }
}