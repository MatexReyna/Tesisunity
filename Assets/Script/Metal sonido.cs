using UnityEngine;

public class SonidoZonaPelota : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonido;

    private void Start()
    {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprobar que lo que tocó es la pelota
        if (collision.gameObject.CompareTag("Pelota"))
        {
            if (audioSource != null && sonido != null)
            {
                audioSource.PlayOneShot(sonido);

                Debug.Log("🏐 ¡La pelota tocó la zona!");
            }
        }
    }
}
