using UnityEngine;

public class ZonaPunto : MonoBehaviour
{
    public bool ladoJugador;

    [Header("Sonido del punto")]
    public AudioSource audioSource;
    public AudioClip sonidoSilbato;

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.loop = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Pelota"))
            return;

        // SONIDO DEL SILBATO
        if (audioSource != null && sonidoSilbato != null)
        {
            audioSource.PlayOneShot(sonidoSilbato);
            Debug.Log("📢 ¡Silbato! ¡Punto!");
        }

        if (ladoJugador)
        {
            gameManager.PuntoBot();
        }
        else
        {
            gameManager.PuntoJugador();
        }
    }
}