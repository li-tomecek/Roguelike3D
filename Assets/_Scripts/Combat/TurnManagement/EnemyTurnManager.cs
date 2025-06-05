
public class EnemyTurnManager : TurnManager
{
    private EnemyUnit unit;
    public EnemyTurnManager(EnemyUnit unit) 
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();
        
        //this must be the last statement in the function
        //unit.UseDefaultSkill( CombatManager.Instance.GetRandomPlayerUnit()); //temp
        unit.UseDefaultSkill( CombatManager.Instance.GetPlayerUnits()[0]); //temp
        EndTurn();
        
    }

}
