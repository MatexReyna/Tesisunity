using UnityEngine;

public class JugadorNv3 : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;

    private Rigidbody2D rb;
    private Vector2 movimiento;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Movimiento con las cuatro flechas
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movimiento = new Vector2(horizontal, vertical).normalized;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movimiento * velocidad;
    }
}