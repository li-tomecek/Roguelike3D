
public class PlayerTurnManager : TurnManager
{
    public PlayerTurnManager(Unit unit) : base(unit)
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    public override void StartTurn()
    {
        base.StartTurn();
        
        //this must be the last statement in the function
        InputController.Instance.SubmitEvent.AddListener(UseDefaultSkill);  //temporary! when the player confirms, the unit will use their default skill
        
    }

    public override void EndTurn()
    {
        InputController.Instance.SubmitEvent.RemoveAllListeners();
        base.EndTurn();
    }
    
    public void UseDefaultSkill()
    {
        unit.UseDefaultSkill(CombatManager.Instance.GetRandomEnemyUnit());
        EndTurn();
    }

}
