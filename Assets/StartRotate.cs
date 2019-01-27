using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRotate : MonoBehaviour
{
    public float rotateSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, -Time.deltaTime * rotateSpeed)));
    }
}
