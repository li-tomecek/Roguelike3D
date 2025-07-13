
public class EnemyUnit : Unit
{
    protected override void Awake()
    {
        turnManager = gameObject.AddComponent<EnemyTurnManager>();
        base.Awake();
    }
    
}
