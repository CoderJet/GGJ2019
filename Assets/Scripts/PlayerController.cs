using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 1f;
    public int maxJumps = 2;

    Animator animator;
    Rigidbody2D rigidbody2D;

    float horizontalMovementDelta = 0f;
    bool isJumping = false;
    int jumpCount = 0;

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
        Vector2 movementDirection = new Vector2(horizontalMovementDelta, 0.0f);
        Vector2 transDirection = transform.TransformDirection(movementDirection) * Time.fixedDeltaTime;
        rigidbody2D.AddForce(transDirection);
    }

    public void SetHorizontalMovementDelta(float movementDelta)
    {
        horizontalMovementDelta = movementDelta * movementSpeed;
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
}