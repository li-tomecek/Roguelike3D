using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public static InputController Instance;
    private GameControls _gameControls;

    private InputActionMap _movementControlsMap;
    private InputActionMap _menuControlsMap;

    //MOVEMENT CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action AttackEvent;

    //MENU CONTROLS
    public event Action<Vector2> NavigateEvent;     //menu selections
    //public event Action SubmitEvent;
    //public event Action CancelEvent;
    public UnityEvent SubmitEvent;                  //If I want to use listeners instead
    public UnityEvent CancelEvent;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            _gameControls = new GameControls();
            _movementControlsMap = _gameControls.Player;
            _menuControlsMap = _gameControls.UI;
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
        _gameControls.Player.Attack.performed += OnAttackPerformed;

        _gameControls.UI.Navigate.performed += OnNavigatePerformed;
        _gameControls.UI.Submit.performed += OnSubmitPerformed;
        _gameControls.UI.Cancel.performed += OnCancelPerformed;
    }
    
    // CHANGE ACTIVE MAP
    public void ActivateMovementMap()
    {
        _movementControlsMap.Enable();
        _menuControlsMap.Disable();
    }
    public void ActivateMenuMap()
    {
        _menuControlsMap.Enable();
        _movementControlsMap.Disable();
    }

    // PLAYER MOVEMENT CONTROLS
    private void OnMovePerformed(InputAction.CallbackContext context) { MoveEvent?.Invoke(context.ReadValue<Vector2>()); }
    private void OnMoveCancelled(InputAction.CallbackContext context) { MoveEvent?.Invoke(Vector2.zero); }
    private void OnJumpPerformed(InputAction.CallbackContext context) { JumpEvent?.Invoke(); }
    private void OnAttackPerformed(InputAction.CallbackContext context) { AttackEvent?.Invoke(); }


    // MENU NAVIGATION CONTROLS
    
    /*public void ReAssignSubmitEvent(Action newFunction)
    {
        SubmitEvent = newFunction;
    }
    public void ReAssignBack(Action newFunction)
    {
        CancelEvent = newFunction;
    }
    public void ReAssignSubmitEventAndBack(Action confirm, Action back)
    {
        SubmitEvent = confirm;
        CancelEvent = back;
    }*/

    private void OnSubmitPerformed(InputAction.CallbackContext context) { SubmitEvent?.Invoke(); }
    private void OnCancelPerformed(InputAction.CallbackContext context) { CancelEvent?.Invoke(); }
    private void OnNavigatePerformed(InputAction.CallbackContext context) { NavigateEvent?.Invoke(context.ReadValue<Vector2>()); }
}
