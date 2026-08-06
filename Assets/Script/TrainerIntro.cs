using UnityEngine;

public class TrainerIntro : MonoBehaviour
{
    [Header("Referencias")]
    public DrakeController drake;
    public NPCMovement luke;
    public CameraFollow cam;

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip sonidoSilbato;

    [Header("Tiempo")]
    public float introDuration = 35f;

    private float timer = 0f;
    private bool finished = false;

    void Awake()
    {
        // Bloquear jugador y Luke
        drake.canMove = false;
        luke.canStart = false;

        // Cámara fija en entrenador
        cam.followPlayer = false;
        cam.target = transform;

        // Zoom intro
        Camera.main.orthographicSize = 7.5f;
    }

    void Update()
    {
        if (finished)
            return;

        timer += Time.deltaTime;

        if (timer >= introDuration)
        {
            FinishIntro();
        }
    }

    void FinishIntro()
    {
        finished = true;

        // 🔊 Reproducir silbato
        if (audioSource != null && sonidoSilbato != null)
        {
            audioSource.PlayOneShot(sonidoSilbato);
        }

        // Activar juego
        drake.canMove = true;
        luke.canStart = true;

        // Cámara vuelve a Drake
        cam.target = drake.transform;
        cam.followPlayer = true;

        // Zoom final
        Camera.main.orthographicSize = 7f;

        // Ocultar entrenador
        gameObject.SetActive(false);
    }
}