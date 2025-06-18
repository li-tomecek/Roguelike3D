public abstract class TurnManager
{
    private Unit unit;
    public TurnManager(Unit unit)
    {
        this.unit = unit;
    }

    public virtual void StartTurn()
    {
        //Resolve any active effects
        Effect effect;
        for (int i = 0; i < unit.GetActiveEffects().Count; i++)
        {
            effect = unit.GetActiveEffects()[i];
            effect.duration--;
            if (effect.duration < 0)    // means that is is still up for one turn
            {
                effect.RemoveEffect(unit);
                unit.GetActiveEffects().Remove(effect);
            }
        }
        
        
        unit.IncrementBP();
    }

    public virtual void EndTurn()
    {
        CombatManager.Instance.NextTurn();
    }

}
