using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;

    void FixedUpdate()
    {
        if (attractor)
        {
            attractor.Attract(transform);
        }
    }
}