using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable Objects/Player/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    //[Header("Movement")]
    [Range(0.0f, 50.0f)]
    public float
        moveSpeedMin,  //base movement speed
        moveSpeedMax,  //max movement speed
        acceleration;  //how much the speed increases per second


    //[Header("Jumping")]
    [Range(0.0f, 50.0f)]
    public float
       jumpHeight,
       gravityForce;
    [Range(0.0f, 1.0f)]
    public float jumpMovementScale;  //how much the lateral movement is scaled when jumping (e.g. 0.5 for half)    
}
