public abstract class TurnManager
{
    //protected Unit unit;
    public TurnManager()
    {
        //this.unit = unit;d
    }

    public virtual void StartTurn()
    {
        //ToDo: resolve any active effects
    }

    public virtual void EndTurn()
    {
        CombatManager.Instance.NextTurn();
    }

}
