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

        if (Input.GetButtonDown("Pickup"))
        {
            if (!playerController.GetHoldingState())
            {
                playerController.PickupObject();
            }
            else
            {
                playerController.DropObject();
            }
        }
    }
}