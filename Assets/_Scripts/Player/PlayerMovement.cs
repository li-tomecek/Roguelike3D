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
    private bool _isGrounded;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        IsGrounded();
    }

    void Update()
    {

        //TODO: look into global input handler and imput maps when you regain access to D2L (instead of handling player input in this script)
        _directionalInput.x = Input.GetAxis("Horizontal");
        _directionalInput.z = Input.GetAxis("Vertical");

        //Look in the direction of the _directionalAxis vector and adjust the movement speed
        if (_directionalInput == Vector3.zero) 
        {
            _speed = 0f;
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(_directionalInput.normalized, Vector3.up); 

            if (_speed == 0)
                _speed = _movementConfig.moveSpeedMin;
            else
                _speed = Mathf.Min(_speed + (_movementConfig.acceleration * Time.deltaTime), _movementConfig.moveSpeedMax);

            _directionalInput *= _speed;
        }
    

        // Check for Jump
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _movementVector.y = Mathf.Sqrt(_movementConfig.jumpHeight * 2f * _movementConfig.gravityForce);   //v_0 = sqrt(2 * gravity * height)
            _isGrounded = false;
        }

        if (!_isGrounded)
        {
            _directionalInput *= _movementConfig.jumpMovementScale;
            _movementVector.y -= _movementConfig.gravityForce * Time.deltaTime;   // continuously add to gravitational acceleration until unit is grounded
            
            if (_movementVector.y < 0 && IsGrounded())  // only check for grounded condition after player starts falling again
                _movementVector.y = 0f;
        } 

        //Move in desired direction
        _movementVector.x = _directionalInput.x;
        _movementVector.z = _directionalInput.z;

        _controller.Move(_movementVector * Time.deltaTime);
    }

    public bool IsGrounded()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.3f);
        return _isGrounded;
    }
}
