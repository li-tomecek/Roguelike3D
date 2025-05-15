using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private MovementConfig _movementConfig;
    [SerializeField] Follower _follower;
    [SerializeField] private bool _isControllable;              //is this the 
    private float _speed;
    private bool _isGrounded;
    private Vector3 _directionalInput;
    private Vector3 _movementVector;

    void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        IsGrounded();
        
        _follower?.SetFollowPoint(transform.Find("FollowerPoint"));
    }

    public void HandleJump()
    {
        if (IsGrounded())
        {
            _movementVector.y = Mathf.Sqrt(_movementConfig.jumpHeight * 2f * _movementConfig.gravityForce);   //v_0 = sqrt(2 * gravity * height)
            _isGrounded = false;
        }
    }

    void Update()
    {
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
                
                _follower?.StartCoroutine(_follower.StartFollowing());      //follower starts moving after you do
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

    public bool IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);
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
}
