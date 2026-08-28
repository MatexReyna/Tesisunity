using UnityEngine;

public class SonidoRed : MonoBehaviour
{
    public AudioSource audioSource;

    private void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Pelota"))
        {
            audioSource.PlayOneShot(audioSource.clip);

            Debug.Log("🏐 ¡La pelota tocó la red!");
        }
    }
}
