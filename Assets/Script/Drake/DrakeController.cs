using UnityEngine;
using System.Collections;

public class DrakeController : MonoBehaviour
{
    public PlayerSoundController playerSoundController;


    [Header("Movimiento")]
    public float speed = 5f;
    public float jumpForce = 7f;

    [Header("Dash")]
    public float dashSpeed = 15f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Control Dialogue")]
    public bool canMove = true;

    private Rigidbody2D rb;
    private Animator anim;

    private float move;

    private bool isGrounded = false;
    private bool canDash = true;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 🔒 BLOQUEO POR DIALOGO
        if (!canMove)
        {
            move = 0f;
            anim.SetFloat("Speed", 0f);
            anim.SetBool("isJumping", false);
            return;
        }

        HandleInput();

        if (move != 0 && isGrounded)
        {
            playerSoundController.PlayCorrer();
        }
        else
        {
            playerSoundController.StopCorrer();
        }
        Jump();

        if ((Input.GetKeyDown(KeyCode.LeftShift) ||
             Input.GetKeyDown(KeyCode.RightShift)) &&
             canDash &&
             isGrounded)
        {
            StartCoroutine(Dash());
        }

        anim.SetFloat("Speed", isDashing ? 1 : Mathf.Abs(move));
        anim.SetBool("isJumping", !isGrounded);
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.x = move * speed;
            rb.linearVelocity = velocity;
        }
    }

    void HandleInput()
    {
        move = 0f;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            move = 1f;
            transform.localScale = new Vector3(1, 1, 1);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            move = -1f;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerSoundController.playSaltar();

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            isGrounded = false;
        }
    }

    IEnumerator Dash()
    {

         playerSoundController.PlayDash();

        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float timer = 0f;

        while (timer < dashDuration)
        {
            float direction = transform.localScale.x;

            rb.linearVelocity = new Vector2(
                direction * dashSpeed,
                rb.linearVelocity.y
            );

            timer += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Valla"))
    {
        playerSoundController.PlayValla();
    }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}