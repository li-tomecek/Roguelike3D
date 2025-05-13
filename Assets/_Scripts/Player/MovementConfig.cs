using UnityEngine;

[CreateAssetMenu(fileName = "MovementConfig", menuName = "Scriptable Objects/Player/MovementConfig")]
public class MovementConfig : ScriptableObject
{
    public float moveSpeedMin, moveSpeedMax, acceleration, jumpForce;
}
