using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    private GameControls _gameControls;
    
    //PLAYER CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _gameControls = new GameControls();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        _gameControls.Player.Enable();

        _gameControls.Player.Move.performed += OnMovePerformed;
        _gameControls.Player.Move.canceled += OnMoveCancelled;
        _gameControls.Player.Jump.performed += OnJumpPerformed;
    }

    // PLAYER CONTROLS
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(Vector2.zero);
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        JumpEvent?.Invoke();
    }
}
