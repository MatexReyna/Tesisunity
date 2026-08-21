using UnityEngine;
using System.Collections;

public class RespawnPelota : MonoBehaviour
{
    [Header("Puntos de saque")]
    public Transform saqueJugador;
    public Transform saqueBot;

    [Header("Centro de la red")]
    public float posicionRed = -329.89f;

    public float tiempoEspera = 1f;

    private Rigidbody2D rb;
    private bool puedeDetectar = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!puedeDetectar)
            return;

        if (!collision.gameObject.CompareTag("Suelo"))
            return;

        puedeDetectar = false;

        if (transform.position.x < posicionRed)
        {
            StartCoroutine(Respawn(saqueBot.position));
        }
        else
        {
            StartCoroutine(Respawn(saqueJugador.position));
        }
    }

    IEnumerator Respawn(Vector3 posicion)
    {
        // Detiene completamente la pelota
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        // La hace "flotar"
        rb.gravityScale = 0;

        // La mueve al saque
        transform.position = posicion;

        // Espera un segundo
        yield return new WaitForSeconds(tiempoEspera);

        // Vuelve la gravedad
        rb.gravityScale = 0.4f;

        // Ya puede volver a detectar el suelo
        puedeDetectar = true;
    }
}