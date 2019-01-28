using UnityEngine;
using System.Collections;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -12;

    public void Attract(Transform body)
    {
        Vector2 gravityUp = (body.position - transform.position).normalized;
        Vector2 localUp = body.up;

        Rigidbody2D attractedRigidbody2D = body.GetComponent<Rigidbody2D>();

        attractedRigidbody2D.AddForce(gravityUp * gravity);

        Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 500f * Time.deltaTime);
    }
}