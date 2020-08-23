using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public InputToken InputToken { get; private set; }

    Vector2 heldDirection;

    // Start is called before the first frame update
    void Start()
    {
        InputToken = new InputToken();
        foreach (var ir in GetComponents<IInputReader>())
            ir.InputToken = InputToken;
    }

    public void OnMove(InputAction.CallbackContext e)
    {
        InputToken.Direction = e.ReadValue<Vector2>();
        if (heldDirection.y > -.5f && InputToken.Direction.y < -.5f)
            InputToken.PressPassThrough();
        heldDirection = InputToken.Direction;
    }

    public void HandleUse(InputAction.CallbackContext e)
    {
        if (e.started)
        {
            InputToken.PressUse();
            InputToken.UseHeld = true;
        }
        else if(e.canceled)
        {
            InputToken.UseHeld = false;
        }
    }

    public void OnJump(InputAction.CallbackContext e)
    {
        if (e.started)
        {
            InputToken.PressJump();
            InputToken.JumpHeld = true;
        }
        else if(e.canceled)
        {
            InputToken.JumpHeld = false;
        }
    }

    public void OnClimb(InputAction.CallbackContext e)
    {
        if (e.started)
            InputToken.ClimbHeld = true;
        else if(e.canceled)
            InputToken.ClimbHeld = false;
    }
}
