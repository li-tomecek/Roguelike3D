using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class TurnManager : MonoBehaviour
{
    protected virtual void StartTurn()
    {
        // resolve and active effects
    }

    protected virtual void EndTurn()
    {
        // go to next turn in sequence
    }
}
