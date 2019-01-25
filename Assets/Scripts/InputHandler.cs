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

        if (Input.GetButtonDown("Crouch"))
        {
            playerController.BeginCrouch();
        }
        else if (Input.GetButtonUp("Crouch"))
        {
            playerController.EndCrouch();
        }
    }
}