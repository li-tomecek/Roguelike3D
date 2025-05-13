using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    private CharacterController _controller;
    [SerializeField] private MovementConfig _movementConfig;

    private Vector3 _nextStep;
    private Vector3 _directionalInput;
    private bool _isGrounded;

    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        //TODO: look into global input handler when you regain access to D2L (instead of handling player input in this script)
        _directionalInput.x = Input.GetAxis("Horizontal");
        _directionalInput.z = Input.GetAxis("Vertical");

        if (_directionalInput == Vector3.zero)
            return;

        //Look in the direction of the _directionalAxis vector
        transform.rotation = Quaternion.LookRotation(_directionalInput.normalized, Vector3.up);

        //Move in desired direction
        _controller.Move(_directionalInput * _movementConfig.moveSpeedMin * Time.deltaTime);
    }

    public void Jump()
    {
        //TODO
    }
}
