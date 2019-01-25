using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerController playerController;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        playerController.xAxisDelta = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            playerController.BeginJump();
        }
    }
}