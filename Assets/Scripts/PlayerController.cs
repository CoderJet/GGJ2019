using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [HideInInspector] public float xAxisDelta = 0f;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isCrouched = false;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheckTransform;
    [SerializeField] float groundCheckRadius = 0f;

    public float movementSpeed = 0f;
    public float jumpForce = 0f;
    public int maxJumps = 2;

    int jumpCount;

    bool isGrounded;
    bool isFacingRight;

    Animator animator;
    Rigidbody2D rigidbody2D;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        jumpCount = 0;
        isFacingRight = true;
    }

    void FixedUpdate()
    {
        bool wasGrounded = isGrounded;
        isGrounded = IsGrounded();

        if (!wasGrounded && isGrounded)
        {
            EndJump();
        }

        float horizontalMovementDelta = (xAxisDelta * movementSpeed) * Time.fixedDeltaTime;

        Move(horizontalMovementDelta, isJumping, isCrouched);
        isJumping = false;
    }

    void Update()
    {
        float animMovementDelta = Mathf.Abs(xAxisDelta * movementSpeed);

        animator.SetFloat("MovementSpeed", animMovementDelta);
        animator.SetBool("IsCrouching", isCrouched);
    }

    void Move(float movementDelta, bool isJumping, bool isCrouched)
    {
        if (isJumping)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }

        if ((movementDelta > 0 && !isFacingRight) || (movementDelta < 0 && isFacingRight))
        {
            Flip();
        }

        rigidbody2D.velocity = new Vector2(movementDelta, rigidbody2D.velocity.y);
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
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

    public void BeginJump()
    {
        if (jumpCount < maxJumps)
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

    public void BeginCrouch()
    {
        isCrouched = true;
    }

    public void EndCrouch()
    {
        isCrouched = false;
    }
}