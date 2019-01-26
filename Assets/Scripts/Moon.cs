using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public float gravityForce;
    public float gravityRadius;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, gravityRadius);
    }

    void FixedUpdate()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, gravityRadius);

        foreach (Collider2D collider in colliders)
        {
            Vector3 directionToward = transform.position - collider.transform.position;

            collider.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(directionToward.x, directionToward.y).normalized * gravityForce * Time.fixedDeltaTime);
        }
    }
}
