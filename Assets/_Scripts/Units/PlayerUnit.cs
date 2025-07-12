
public class PlayerUnit : Unit
{
    protected override void Start()
    {
        turnManager = gameObject.AddComponent<PlayerTurnManager>();
        base.Start();
    }
    
    public PlayerTurnManager GetPlayerTurnManager()
    {
        return (PlayerTurnManager)turnManager;
    }
}
