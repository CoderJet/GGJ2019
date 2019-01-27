using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public Vector3 pickupHoldPosition;
    public Vector2 reachBoxSize;
    public Vector3 defaultCrosshairPosition;
    public Transform reachOffsetTransform;
    public Transform groundCheckTransform;
    public Transform crosshairTransform;
    public LayerMask groundLayer;
    public LayerMask pickupLayer;

    public float groundCheckRadius = 0.2f;
    public float movementSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float maxVelocity = 1000f;
    public float firingForce = 5f;

    public int maxJumps = 2;

    Animator animator;
    Rigidbody2D rigidbody2D;

    Collider2D[] pickupColliders;

    float horizontalMovementDelta = 0f;

    GameObject currentlyHeldObject;

    int jumpCount = 0;

    bool isAiming = false;
    bool isJumping = false;
    bool isGrounded = false;
    bool isFacingRight = true;
    bool isHoldingObject = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        pickupColliders = Physics2D.OverlapBoxAll(reachOffsetTransform.position, reachBoxSize, 0, pickupLayer);
        animator.SetFloat("Speed", Mathf.Abs(horizontalMovementDelta));
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

        if (!isAiming)
        {
            Vector2 transDirection = transform.TransformDirection(movementDirection) * Time.fixedDeltaTime;
            rigidbody2D.AddForce(transDirection);
            rigidbody2D.velocity = new Vector2(Mathf.Clamp(rigidbody2D.velocity.x, maxVelocity * -1, maxVelocity), Mathf.Clamp(rigidbody2D.velocity.y, maxVelocity * -1, maxVelocity));
        }
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
        Gizmos.DrawWireCube(reachOffsetTransform.position, reachBoxSize);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + pickupHoldPosition, .2f);
    }

    public void PickupObject()
    {
        foreach (Collider2D pickupCollider in pickupColliders)
        {
            if (pickupCollider.gameObject != gameObject)
            {
                GameObject pickupObject = pickupCollider.gameObject;
                foreach (Collider c in pickupObject.GetComponents<Collider>())
                {
                    c.enabled = false;
                }

                pickupObject.GetComponent<Rigidbody2D>().isKinematic = true;
                pickupObject.transform.parent = gameObject.transform;
                pickupObject.transform.localPosition = pickupHoldPosition;

                isHoldingObject = true;
                currentlyHeldObject = pickupObject;
            }
        }
    }

    public void DropObject()
    {
        currentlyHeldObject.GetComponent<Rigidbody2D>().isKinematic = false;
        currentlyHeldObject.transform.parent = null;
        isHoldingObject = false;
        currentlyHeldObject = null;
    }

    public bool GetHoldingState()
    {
        return isHoldingObject;
    }

    public void BeginAim()
    {
        if (isGrounded)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0.0f;
            isAiming = true;
        }
    }

    public void StopAim()
    {
        isAiming = false;
        crosshairTransform.localPosition = defaultCrosshairPosition;
    }

    public void FireHeldObject()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pickupHoldPosition);
        Vector3 direction = (crosshairTransform.position - screenPosition).normalized;
        currentlyHeldObject.GetComponent<Rigidbody2D>().isKinematic = false;
        isHoldingObject = false;
        currentlyHeldObject.transform.parent = null;
        currentlyHeldObject.GetComponent<Rigidbody2D>().AddForce(direction * firingForce);
        currentlyHeldObject = null;
        StopAim();
    }

    public void MoveCrosshair(Vector2 directionalInput)
    {
        crosshairTransform.localPosition += new Vector3(directionalInput.x, directionalInput.y, 0) * Time.deltaTime;
    }
}