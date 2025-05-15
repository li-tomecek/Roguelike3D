using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    private GameControls _gameControls;
    
    //PLAYER CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    void Awake()
    {
        _gameControls = new GameControls();
    }

    void OnEnable()
    {
        _gameControls.Player.Enable();

        _gameControls.Player.Move.performed += OnMovePerformed;
        _gameControls.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }
}
