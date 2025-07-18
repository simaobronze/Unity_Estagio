using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NinjaInputController : MonoBehaviour
{
    public Vector2 MoveInputVector { get; private set; }

    public event Action OnJumpButtonPressed;
    private void OnMove(InputValue inputValue)
    {
        MoveInputVector = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            OnJumpButtonPressed?.Invoke();
        }
    }
}
