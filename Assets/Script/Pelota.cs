using UnityEngine;

public class Pelota : MonoBehaviour
{
    public float fuerzaHorizontal = 8f;
    public float fuerzaVertical = 6f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float direccion;

            // Si el jugador está a la izquierda, la pelota va a la derecha
            if (collision.transform.position.x < transform.position.x)
                direccion = 1f;
            else
                direccion = -1f;

            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            rb.AddForce(new Vector2(direccion * fuerzaHorizontal, fuerzaVertical),
                        ForceMode2D.Impulse);
        }
    }
}
