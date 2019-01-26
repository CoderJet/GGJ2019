using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rigidbody2D;

    public float movementSpeed;

    Vector2 moveDirection;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        Vector2 transDirection = transform.TransformDirection(moveDirection) * movementSpeed * Time.deltaTime;
        rigidbody2D.AddForce(transDirection);
    }
}