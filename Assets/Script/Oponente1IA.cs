using UnityEngine;

public class Oponente1IA : MonoBehaviour
{
    [Header("Referencias")]
    public Transform pelota;
    public Transform puntoAgarreOponente;

    [Header("Movimiento")]
    public float velocidad = 3f;
    public float velocidadEsquiva = 5f;
    public float velocidadBuscarPelota = 4f;

    [Header("Esquiva")]
    public float distanciaDeteccion = 6f;

    [Header("Cambio de dirección")]
    public float tiempoCambioDireccion = 1.5f;

    [Header("Distancia para agarrar")]
    public float distanciaAgarrar = 0.8f;

    [Header("Límites de su mitad")]
    public float limiteIzquierdo = 0.8f;
    public float limiteDerecho = 9.5f;
    public float limiteAbajo = -3.8f;
    public float limiteArriba = 3.5f;

    [Header("Animaciones")]
    public Animator animator;

    private Rigidbody2D rb;
    private Rigidbody2D rbPelota;

    private Vector2 movimiento;
    private float temporizador;

    private bool buscandoPelota = false;
    private bool tienePelota = false;
    private bool esquivando = false;

    private string animacionActual = "";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        if (pelota != null)
        {
            rbPelota = pelota.GetComponent<Rigidbody2D>();
        }

        ElegirDireccion();

        temporizador = tiempoCambioDireccion;

        CambiarAnimacion("StateO3");
    }

    void Update()
    {
        esquivando = false;

        // Si tiene la pelota
        if (tienePelota)
        {
            rb.linearVelocity = Vector2.zero;

            CambiarAnimacion("StateO3");

            return;
        }

        // Si la pelota viene hacia el oponente
        if (pelota != null && rbPelota != null)
        {
            if (PelotaVieneHaciaMi())
            {
                buscandoPelota = false;
                esquivando = true;

                Esquivar();

                CambiarAnimacion("CrouchO3");

                return;
            }
        }

        // Si la pelota está en su mitad
        if (pelota != null && PelotaEnMiMitad())
        {
            float distancia = Vector2.Distance(
                transform.position,
                pelota.position
            );

            if (distancia > distanciaAgarrar)
            {
                buscandoPelota = true;
            }
        }

        // Buscar la pelota
        if (buscandoPelota)
        {
            BuscarPelota();

            CambiarAnimacion("RunO3");

            return;
        }

        // Movimiento normal
        temporizador -= Time.deltaTime;

        if (temporizador <= 0)
        {
            ElegirDireccion();

            temporizador = tiempoCambioDireccion;
        }

        // Animación según movimiento
        if (movimiento != Vector2.zero)
        {
            CambiarAnimacion("RunO3");
        }
        else
        {
            CambiarAnimacion("StateO3");
        }
    }

    void FixedUpdate()
    {
        if (!tienePelota)
        {
            rb.linearVelocity = movimiento * VelocidadActual();
        }

        LimitarMovimiento();
    }

    float VelocidadActual()
    {
        if (buscandoPelota)
        {
            return velocidadBuscarPelota;
        }

        if (esquivando)
        {
            return velocidadEsquiva;
        }

        return velocidad;
    }

    bool PelotaVieneHaciaMi()
    {
        if (pelota == null || rbPelota == null)
        {
            return false;
        }

        float distancia = Vector2.Distance(
            pelota.position,
            transform.position
        );

        if (distancia > distanciaDeteccion)
        {
            return false;
        }

        if (rbPelota.linearVelocity.magnitude < 0.5f)
        {
            return false;
        }

        Vector2 direccionPelota =
            rbPelota.linearVelocity.normalized;

        Vector2 haciaOponente =
            (transform.position - pelota.position).normalized;

        float direccion = Vector2.Dot(
            direccionPelota,
            haciaOponente
        );

        return direccion > 0.4f;
    }

    void Esquivar()
    {
        float diferenciaY =
            pelota.position.y - transform.position.y;

        if (diferenciaY > 0.2f)
        {
            movimiento = Vector2.down;
        }
        else if (diferenciaY < -0.2f)
        {
            movimiento = Vector2.up;
        }
        else
        {
            if (Random.value > 0.5f)
            {
                movimiento = Vector2.up;
            }
            else
            {
                movimiento = Vector2.down;
            }
        }
    }

    void BuscarPelota()
    {
        Vector2 direccion =
            (pelota.position - transform.position).normalized;

        movimiento = direccion;

        float distancia = Vector2.Distance(
            transform.position,
            pelota.position
        );

        if (distancia <= distanciaAgarrar)
        {
            AgarrarPelota();
        }
    }

    bool PelotaEnMiMitad()
    {
        if (pelota == null)
        {
            return false;
        }

        return pelota.position.x >= limiteIzquierdo;
    }

    void AgarrarPelota()
    {
        tienePelota = true;
        buscandoPelota = false;

        rb.linearVelocity = Vector2.zero;

        rbPelota.linearVelocity = Vector2.zero;

        rbPelota.simulated = false;

        pelota.SetParent(puntoAgarreOponente);

        pelota.position = puntoAgarreOponente.position;
    }

    void ElegirDireccion()
    {
        float x = Random.Range(-1f, 1f);
        float y = Random.Range(-1f, 1f);

        movimiento = new Vector2(x, y).normalized;
    }

    void LimitarMovimiento()
    {
        Vector3 posicion = transform.position;

        posicion.x = Mathf.Clamp(
            posicion.x,
            limiteIzquierdo,
            limiteDerecho
        );

        posicion.y = Mathf.Clamp(
            posicion.y,
            limiteAbajo,
            limiteArriba
        );

        transform.position = posicion;
    }

    void CambiarAnimacion(string nombre)
    {
        if (animator == null)
        {
            return;
        }

        // No reinicia la misma animación constantemente
        if (animacionActual == nombre)
        {
            return;
        }

        animator.Play(nombre);

        animacionActual = nombre;
    }
}