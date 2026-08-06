using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public float stopPoint = 5f;

    [Header("Salto")]
    public float jumpForce = 7f;
    public float jumpTime = 5f;

    [Header("Final")]
    public float finalStopTime = 23f;

    [Header("Control Dialogue")]
    public bool canStart = false;

    private bool stopped = false;
    private bool isGrounded = true;

    private float jumpTimer;
    private float totalTimer;

    private Rigidbody2D rb;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpTimer = jumpTime;

        anim.SetFloat("Speed", 0f);
        anim.SetBool("isJumping", false);
    }

    void Update()
    {
        if (!canStart)
        {
            anim.SetFloat("Speed", 0f);
            return;
        }

        totalTimer += Time.deltaTime;

        if (totalTimer >= finalStopTime)
        {
            stopped = true;
            anim.SetFloat("Speed", 0f);
            return;
        }

        // Correr
        if (!stopped)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);

            if (isGrounded)
            {
                anim.SetFloat("Speed", 1f);
            }
        }

        // Salto
        jumpTimer -= Time.deltaTime;

        if (jumpTimer <= 0f && isGrounded)
        {
            Jump();
            jumpTimer = jumpTime;
        }
    }

    void Jump()
    {
        isGrounded = false;

        anim.SetBool("isJumping", true);

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            anim.SetBool("isJumping", false);

            if (!stopped)
            {
                anim.SetFloat("Speed", 1f);
            }
        }
    }

    public void StartRace()
    {
        canStart = true;
        anim.SetFloat("Speed", 1f);
    }
}