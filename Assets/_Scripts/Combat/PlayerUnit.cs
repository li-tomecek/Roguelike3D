using UnityEngine;

public class PlayerUnit : Unit
{
    protected override void Start()
    {
        turnManager = new PlayerTurnManager(this);
        base.Start();
    }
    
    public PlayerTurnManager GetPlayerTurnManager()
    {
        return (PlayerTurnManager)turnManager;
    }
}
