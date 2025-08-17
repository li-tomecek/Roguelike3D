using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputController : Singleton<InputController>
{
    private ControlScheme _controlScheme;

    private InputActionMap _movementControlsMap;
    private InputActionMap _menuControlsMap;
    private InputActionMap _activeMap;

    //MOVEMENT CONTROLS
    public event Action<Vector2> MoveEvent;
    public event Action JumpEvent;
    public event Action AttackEvent;
    public event Action InteractEvent;

    //MENU CONTROLS
    public UnityEvent<Vector2> NavigateEvent;       //menu selections
    public UnityEvent SubmitEvent;                  //If I want to use listeners instead
    public UnityEvent CancelEvent;

    public override void Awake()
    {
        base.Awake();

        _controlScheme = new ControlScheme();
        _movementControlsMap = _controlScheme.Player;
        _menuControlsMap = _controlScheme.UI;
    }

    void OnEnable()
    {
        _controlScheme.Player.Enable();

        _controlScheme.Player.Move.performed += OnMovePerformed;
        _controlScheme.Player.Move.canceled += OnMoveCancelled;
        _controlScheme.Player.Jump.performed += OnJumpPerformed;
        _controlScheme.Player.Attack.performed += OnAttackPerformed;
        _controlScheme.Player.Interact.performed += OnInteractPerformed;

        _controlScheme.UI.Navigate.performed += OnNavigatePerformed;
        _controlScheme.UI.Submit.performed += OnSubmitPerformed;
        _controlScheme.UI.Cancel.performed += OnCancelPerformed;
    }

    public void Start()
    {
        CombatManager.Instance.OnCombatStart.AddListener(ActivateMenuMap);
        CombatManager.Instance.OnCombatWin.AddListener(ActivateMovementMap);
    }

    // CHANGE ACTIVE MAP
    public void DisableActiveMap()
    {
        if (_activeMap != null)
            _activeMap.Disable();
    }
    public void EnableActiveMap()
    {
        if(_activeMap != null)
            _activeMap.Enable();
    }
    public void ActivateMovementMap()
    {
        _movementControlsMap.Enable();
        _menuControlsMap.Disable();

        _activeMap = _movementControlsMap;
    }
    public void ActivateMenuMap()
    {
        _menuControlsMap.Enable();
        _movementControlsMap.Disable();

        _activeMap = _menuControlsMap;

    }

    // PLAYER MOVEMENT CONTROLS
    private void OnMovePerformed(InputAction.CallbackContext context) { MoveEvent?.Invoke(context.ReadValue<Vector2>()); }
    private void OnMoveCancelled(InputAction.CallbackContext context) { MoveEvent?.Invoke(Vector2.zero); }
    private void OnJumpPerformed(InputAction.CallbackContext context) { JumpEvent?.Invoke(); }
    private void OnAttackPerformed(InputAction.CallbackContext context) { AttackEvent?.Invoke(); }
    private void OnInteractPerformed(InputAction.CallbackContext context) {InteractEvent?.Invoke(); }


    // MENU NAVIGATION CONTROLS
    private void OnSubmitPerformed(InputAction.CallbackContext context) { SubmitEvent?.Invoke(); }
    private void OnCancelPerformed(InputAction.CallbackContext context) { CancelEvent?.Invoke(); }
    private void OnNavigatePerformed(InputAction.CallbackContext context) { NavigateEvent?.Invoke(context.ReadValue<Vector2>()); }
}
