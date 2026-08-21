using UnityEngine;

public class Pelota : MonoBehaviour
{
    [Header("Fuerza del contacto")]
    public float fuerzaHorizontal = 8f;
    public float fuerzaVertical = 6f;

    private Rigidbody2D rb;
    private GameManager gameManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // TOQUE DEL JUGADOR
        if (collision.gameObject.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                gameManager.ToqueJugador();
            }

            MoverPelota(collision);
        }

        // TOQUE DEL BOT
        else if (collision.gameObject.CompareTag("Bot"))
        {
            if (gameManager != null)
            {
                gameManager.ToqueBot();
            }

            MoverPelota(collision);
        }
    }

    void MoverPelota(Collision2D collision)
    {
        float direccion;

        // Si el personaje está a la izquierda de la pelota,
        // la pelota va hacia la derecha.
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