using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private MovementConfig _movementConfig;
    private float _speed;
    private Vector3 _directionalInput;
    private Vector3 _movementVector;

    [SerializeField] Transform _followerPoint;
    [SerializeField] Transform _projectileOrigin;

    [Header ("Jump")]
    [SerializeField] private float _raycastHeight;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;
    void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        IsGrounded();
    }

    void Update()
    {
        if (CombatManager.Instance.InCombat())
            return;

        //1. Set Movement Speed and Direction
        if (_directionalInput == Vector3.zero) 
        {
            _speed = 0f;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(_directionalInput.normalized, Vector3.up);     //rotate player

            if (_speed == 0)
            {
                _speed = _movementConfig.moveSpeedMin;
                
            }
            else
                _speed = Mathf.Min(_speed + (_movementConfig.acceleration * Time.deltaTime), _movementConfig.moveSpeedMax);
        }
        
        _movementVector.x = _directionalInput.x * _speed;
        _movementVector.z = _directionalInput.z * _speed;
    

        //2. Handle Jump
        if (!_isGrounded)
        {
            _movementVector.x *= _movementConfig.jumpMovementScale;                 //damp directional movement
            _movementVector.z *= _movementConfig.jumpMovementScale;
           
            _movementVector.y -= _movementConfig.gravityForce * Time.deltaTime;     // continuously add to gravitational acceleration until unit is grounded
            
            if (_movementVector.y < 0 && IsGrounded())                              // Ground check after player starts falling again
                _movementVector.y = 0f;
        } 

        //3. Move Character
        _controller.Move(_movementVector * Time.deltaTime);
    }

    public void HandleJump()
    {
        if (IsGrounded())
        {
            _movementVector.y = Mathf.Sqrt(_movementConfig.jumpHeight * 2f * _movementConfig.gravityForce);   //v_0 = sqrt(2 * gravity * height)
            _isGrounded = false;
        }
    }

    public bool IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _raycastHeight, _groundLayer);
        return _isGrounded;
    }

    public void SetDirectionalInput(Vector2 directionalInput)
    {
        _directionalInput.x = directionalInput.x;
        _directionalInput.z = directionalInput.y;
        
        _directionalInput.Normalize();
    }

    public void SetDirectionalInput(Vector3 directionalInput)
    {
        _directionalInput = directionalInput.normalized;
        _directionalInput.y = 0f;
    }

    public Transform GetProjectileOrigin() { return _projectileOrigin; }
    public Transform GetFollowerPoint() { return _followerPoint; }
}



