
public class EnemyTurnManager : TurnManager
{
    public EnemyTurnManager(Unit unit) : base(unit)
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    public override void StartTurn()
    {
        base.StartTurn();
        
        //this must be the last statement in the function
        unit.UseDefaultSkill(CombatManager.Instance.GetRandomPlayerUnit()); //temp
        EndTurn();
        
    }

}
