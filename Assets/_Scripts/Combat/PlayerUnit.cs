using UnityEngine;

public class PlayerUnit : Unit
{
    protected override void Start()
    {
        turnManager = new PlayerTurnManager(this);
        base.Start();
    }
}
