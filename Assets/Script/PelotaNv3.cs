using UnityEngine;

public class PelotaNv3 : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugador;

    [Header("Agarrar")]
    public float distanciaAgarrar = 2f;

    [Header("Lanzamiento")]
    public float fuerzaLanzamiento = 12f;

    [Header("Posición en la mano")]
    public Vector3 posicionEnMano = new Vector3(0.7f, 0.2f, 0f);

    private Rigidbody2D rb;
    private bool agarrada = false;
    private Vector2 ultimaDireccion = Vector2.right;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (jugador == null)
            return;

        // Dirección del jugador
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 direccion = new Vector2(horizontal, vertical);

        if (direccion != Vector2.zero)
        {
            ultimaDireccion = direccion.normalized;
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

        // Mantener pelota en la mano
        if (agarrada)
        {
            transform.position = jugador.position + posicionEnMano;
        }
    }

    void IntentarAgarrar()
    {
        float distancia = Vector2.Distance(
            transform.position,
            jugador.position
        );

        Debug.Log("Distancia a la pelota: " + distancia);

        if (distancia <= distanciaAgarrar)
        {
            agarrada = true;

            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;

            transform.SetParent(jugador);
            transform.localPosition = posicionEnMano;

            Debug.Log("🏐 ¡PELota AGARRADA!");
        }
        else
        {
            Debug.Log("❌ Estoy demasiado lejos de la pelota.");
        }
    }

    void Lanzar()
    {
        agarrada = false;

        transform.SetParent(null);

        rb.simulated = true;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        rb.AddForce(
            ultimaDireccion * fuerzaLanzamiento,
            ForceMode2D.Impulse
        );

        Debug.Log("🏐💨 ¡PELOTA LANZADA!");
    }
}