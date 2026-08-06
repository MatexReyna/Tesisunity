using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Players")]
    public DrakeController drake;
    public NPCMovement luke;

    [Header("Camera")]
    public CameraFollow cam;
    public Transform trainer;

    [Header("UI")]
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI countdownText;

    [Header("Settings")]
    public float autoTime = 7f;

    private string[] dialogues = new string[]
    {
        "¡Bienvenido, jugador!",
        "Yo soy el entrenador González y este es el Nivel 1 de 'The School Athletics'.",
        "Aquí te enfrentarás en una carrera contra Luke.",
        "Recuerda que el atletismo no solo se trata de correr rápido. La coordinación, la concentración y la práctica son fundamentales para mejorar cada día.",
        "El deporte nos enseña disciplina, esfuerzo y trabajo constante. Cada entrenamiento es una oportunidad para superarnos.",
        "Con la flecha derecha avanzas, con la izquierda retrocedes, con la barra espaciadora saltas y con SHIFT puedes obtener un impulso de velocidad.",
        "Importante: puedes combinar SHIFT y ESPACIO para impulsarte y luego saltar, pero no puedes hacer ESPACIO y después SHIFT en el aire.",
        "¡Espero que haya quedado todo claro! Ahora demuestra tus habilidades y... ¡a ganar!"
    };

    private int index = 0;
    private bool finished = false;

    void Start()
    {
        // 🔒 BLOQUEO INICIAL
        drake.canMove = false;
        luke.canStart = false;

        // 📷 cámara en entrenador
        cam.followPlayer = false;
        cam.target = trainer;

        cam.transform.position = new Vector3(
            trainer.position.x,
            trainer.position.y + cam.offsetY,
            -10f
        );

        // 🧾 UI segura
        countdownText.text = "";
        index = 0;
        finished = false;

        if (dialogues.Length > 0 && dialogText != null)
            dialogText.text = dialogues[0];

        StartCoroutine(DialogueRoutine());
    }

    void Update()
    {
        if (finished) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            NextDialogue();
        }
    }

    IEnumerator DialogueRoutine()
    {
        float timer = 0f;

        while (!finished)
        {
            timer += Time.deltaTime;

            if (timer >= autoTime)
            {
                NextDialogue();
                timer = 0f;
            }

            yield return null;
        }
    }

    void NextDialogue()
    {
        index++;

        if (dialogues == null || index >= dialogues.Length)
        {
            finished = true;
            StartCoroutine(Countdown());
            return;
        }

        if (dialogText != null)
            dialogText.text = dialogues[index];
    }

    IEnumerator Countdown()
    {
        string[] numbers = { "3", "2", "1", "GO!" };

        foreach (string n in numbers)
        {
            if (countdownText != null)
                countdownText.text = n;

            yield return new WaitForSeconds(1f);
        }

        if (countdownText != null)
            countdownText.text = "";

        EndGame();
    }

    void EndGame()
    {
        trainer.gameObject.SetActive(false);

        drake.canMove = true;
        luke.canStart = true;

        cam.followPlayer = true;
        cam.target = drake.transform;
    }
}