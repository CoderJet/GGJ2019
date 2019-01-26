using UnityEngine;
using System.Collections;

public class GravityBody : MonoBehaviour
{
    public GravityAttractor attractor;
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
    }

    void FixedUpdate()
    {
        if (attractor)
        {
            attractor.Attract(myTransform);
        }
    }
}