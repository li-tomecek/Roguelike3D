
public class PlayerUnit : Unit
{
    protected override void Awake()
    {
        turnManager = gameObject.AddComponent<PlayerTurnManager>();
        base.Awake();
    }
    
    public PlayerTurnManager GetPlayerTurnManager()
    {
        return (PlayerTurnManager)turnManager;
    }
}
