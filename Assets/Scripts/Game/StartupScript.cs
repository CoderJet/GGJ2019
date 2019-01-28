using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartupScript : MonoBehaviour
{
    public Canvas c;
    public Cinemachine.CinemachineVirtualCamera startCamera;
    public StartRotate rot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            c.enabled = false;
            startCamera.Priority = -1000;
            rot.enabled = false;
        }
    }
}
