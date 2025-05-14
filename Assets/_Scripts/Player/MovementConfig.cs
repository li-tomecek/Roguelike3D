using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable Objects/Player/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    [Range(0.0f, 50.0f)]
    public float
        moveSpeedMin,  //base movement speed
        moveSpeedMax,  //max movement speed
        acceleration,  //how much the speed increases per second
        jumpHeight,
        gravityForce;

    [Range(0.0f, 1.0f)]
    public float jumpMovementScale;  //how much the lateral movement is scaled when jumping (e.g. 0.5 for half)    


}
