using UnityEngine;

public class EnemyUnit : Unit
{
    protected override void Start()
    {
        turnManager = new EnemyTurnManager(this);
        base.Start();
    }
    
}
