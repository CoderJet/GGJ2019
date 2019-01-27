using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class InputHandler : MonoBehaviour
{
    public PlayerController playerController;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        playerController.SetHorizontalMovementDelta(Input.GetAxisRaw("Horizontal"));

        bool isHoldingItem = playerController.GetHoldingState();

        if (Input.GetButtonDown("Jump"))
        {
            playerController.BeginJump();
        }

        if (Input.GetButtonDown("Pickup"))
        {
            if (!isHoldingItem)
            {
                playerController.PickupObject();
            }
            else
            {
                playerController.DropObject();
            }
        }

        if (Input.GetButtonDown("Interact"))
        {
            if (isHoldingItem)
            {
                playerController.AddItemToInventory();
            }
        }

        if (isHoldingItem)
        {
            if (Input.GetAxis("Aim") > 0)
            {
                playerController.BeginAim();

                playerController.MoveCrosshair(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

                if (Input.GetButtonDown("Fire1") || Input.GetAxis("Fire1") > 0)
                {
                    playerController.FireHeldObject();
                }
            }
            else if (Input.GetAxis("Aim") <= 0)
            {
                playerController.StopAim();
            }
        }
    }
}