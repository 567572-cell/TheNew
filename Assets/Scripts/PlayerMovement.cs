using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float JumpPower;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    // 🟢 New double-jump variables
    [SerializeField] private int maxJumps = 2; // total jumps allowed (2 = double jump)
    private int jumpCount;                     // tracks current number of jumps

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // 🟢 Reset jump count when grounded or on wall
        if (isGrounded() || onWall())
            jumpCount = 0;

        // wall jump logic
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
                body.gravityScale = 3;

            if (Input.GetKeyDown(KeyCode.Space)) // changed to GetKeyDown for cleaner jump input
                jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void jump()
    {
        // 🟢 Ground jump
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, JumpPower);
            anim.SetTrigger("jump");
            jumpCount++;
        }
        // 🟢 Wall jump
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);

            wallJumpCooldown = 0;
        }
        // 🟢 Double jump (when not grounded and jump count < max)
        else if (jumpCount < maxJumps)
        {
            body.velocity = new Vector2(body.velocity.x, JumpPower);
            anim.SetTrigger("jump");
            jumpCount++;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
            Vector2.down, 0.1f, groundlayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0,
            new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
