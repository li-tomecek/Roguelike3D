
public class EnemyUnit : Unit
{
    protected override void Start()
    {
        turnManager = gameObject.AddComponent<EnemyTurnManager>();
        base.Start();
    }
    
}
