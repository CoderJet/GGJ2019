using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public GameObject crosshair;
    public Vector3 pickupHoldPosition;
    public Vector2 reachBoxSize;
    public Vector2 crosshairMaximums;
    public Vector2 crosshairMinimums;
    public Transform reachOffsetTransform;
    public Transform groundCheckTransform;
    public LayerMask groundLayer;
    public LayerMask pickupLayer;

    public float crosshairSpeed = 1f;
    public float groundCheckRadius = 0.2f;
    public float movementSpeed = 1.0f;
    public float jumpForce = 1.0f;
    public float maxVelocity = 1000f;
    public float firingForce = 5f;

    public int maxJumps = 2;

    public AudioSource sounder;
    //public AudioClip 

    Animator animator;
    Rigidbody2D rigidbody2D;

    Collider2D[] pickupColliders;

    float horizontalMovementDelta = 0f;

    public GameObject currentlyHeldObject;

    int jumpCount = 0;

    bool isAiming = false;
    bool isJumping = false;
    bool isGrounded = false;
    bool isFacingRight = true;
    public bool isHoldingObject = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        crosshair.SetActive(false);
    }

    void Update()
    {
        pickupColliders = Physics2D.OverlapBoxAll(reachOffsetTransform.position, reachBoxSize, 0, pickupLayer);
        if (!isAiming)
        {
            animator.SetFloat("Speed", Mathf.Abs(horizontalMovementDelta));
        }
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
        if (!isAiming)
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
        animator.SetTrigger("Pickup");

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
            break;
        }

        //animator.SetBool("Pickup", false);
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
        if (isGrounded && !isAiming)
        {
            crosshair.SetActive(true);
            Rigidbody2D crosshairRigidbody = crosshair.GetComponent<Rigidbody2D>();
            crosshairRigidbody.rotation = rigidbody2D.rotation;
            crosshairRigidbody.position = rigidbody2D.position;
            crosshairRigidbody.freezeRotation = true;
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0.0f;
            isAiming = true;
        }
    }

    public void StopAim()
    {
        isAiming = false;
        crosshair.SetActive(false);
    }

    public void FireHeldObject()
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(pickupHoldPosition);
        Vector2 direction = (crosshair.transform.position - pickupHoldPosition).normalized;
        currentlyHeldObject.GetComponent<Rigidbody2D>().isKinematic = false;
        isHoldingObject = false;
        currentlyHeldObject.transform.parent = null;
        currentlyHeldObject.GetComponent<Rigidbody2D>().AddForce(direction * firingForce * Time.deltaTime, ForceMode2D.Impulse);
        currentlyHeldObject = null;
        StopAim();
    }

    public void MoveCrosshair(float xPos, float yPos)
    {
        float radians = rigidbody2D.rotation * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        Rigidbody2D crosshairRigidbody = crosshair.GetComponent<Rigidbody2D>();

        float newXPos = crosshairRigidbody.position.x + xPos * Time.deltaTime * crosshairSpeed;
        float newYPos = crosshairRigidbody.position.y + yPos * Time.deltaTime * crosshairSpeed;

        float xClampMin = rigidbody2D.position.x + crosshairMinimums.x;
        float xClampMax = rigidbody2D.position.x + crosshairMaximums.x;

        float yClampMin = rigidbody2D.position.y + crosshairMinimums.y;
        float yClampMax = rigidbody2D.position.y + crosshairMaximums.y;

        float clampedXPos = Mathf.Clamp(newXPos, AdjustXToRotation(xClampMin, yClampMin), AdjustXToRotation(xClampMax, yClampMin));
        float clampedYPos = Mathf.Clamp(newYPos, AdjustYToRotation(yClampMin, xClampMin), AdjustYToRotation(yClampMax, xClampMin));

        if ((crosshairRigidbody.position.x < rigidbody2D.position.x) && isFacingRight || (crosshairRigidbody.position.x > rigidbody2D.position.x) && !isFacingRight)
        {
            FlipPlayer();
        }

        Vector2 newCrosshairPosition = new Vector2(clampedXPos, clampedYPos);
        crosshairRigidbody.MovePosition(newCrosshairPosition);
    }
    
    public float AdjustXToRotation(float x, float y)
    {
        float radians = rigidbody2D.rotation * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return cos * x - sin * y;
    }

    public float AdjustYToRotation(float y, float x)
    {
        float radians = rigidbody2D.rotation * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return sin * x + cos * y;
    }
}