using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public Transform groundCheckTransform;
    public LayerMask groundLayer;

    public float groundCheckRadius = 0.2f;
    public float movementSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float maxVelocity = 1000f;
    public float pickupReachRadius = 5f;

    public int maxJumps = 2;

    Animator animator;
    Rigidbody2D rigidbody2D;

    float horizontalMovementDelta = 0f;

    int jumpCount = 0;

    bool isJumping = false;
    bool isGrounded = false;
    bool isFacingRight = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        if (!wasGrounded && isGrounded)
        {
            EndJump();
        }

        Move(horizontalMovementDelta, isJumping);
        isJumping = false;
    }

    public void SetHorizontalMovementDelta(float movementDelta)
    {
        horizontalMovementDelta = movementDelta * movementSpeed;
    }

    public void BeginJump()
    {
        if ((jumpCount < maxJumps) || (maxJumps == 1 && isGrounded))
        {
            jumpCount++;
            isJumping = true;
            animator.SetBool("IsJumping", true);
        }
    }

    public void EndJump()
    {
        jumpCount = 0;
        isJumping = false;
        animator.SetBool("IsJumping", false);
    }

    bool IsGrounded()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckTransform.position, groundCheckRadius, groundLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject)
            {
                return true;
            }
        }

        return false;
    }

    void Move(float movementDelta, bool isJumping)
    {
        Vector2 movementDirection = new Vector2(movementDelta, 0.0f);

        if (isJumping)
        {
            movementDirection.y = jumpForce;
        }

        if ((movementDelta > 0 && !isFacingRight) || (movementDelta < 0 && isFacingRight))
        {
            FlipPlayer();
        }

        if (!isGrounded)
        {
            movementDirection.x = 0;
        }

        Vector2 transDirection = transform.TransformDirection(movementDirection) * Time.fixedDeltaTime;
        rigidbody2D.AddForce(transDirection);
        rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, maxVelocity * -1, maxVelocity), Mathf.Clamp(rigidbody2D.velocity.y, maxVelocity * -1, maxVelocity));
    }

    void FlipPlayer()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheckTransform.position, groundCheckRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, pickupReachRadius);
    }

    public void PickupObject()
    {

    }
}