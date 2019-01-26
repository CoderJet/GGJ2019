using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour
{
    public PlayerController playerController;

    void FixedUpdate()
    {
        HandleInput();
    }

    void HandleInput()
    {
        playerController.SetHorizontalMovementDelta(Input.GetAxisRaw("Horizontal"));

        if (Input.GetButtonDown("Jump"))
        {
            playerController.BeginJump();
        }
    }
}