using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TurnManager
{
    private Unit _unit;

    public TurnManager(Unit unit)
    {
        _unit = unit;
    }

    public void StartTurn()
    {
        // resolve any active effects
        _unit.UseDefaultSkill(CombatManager.Instance.GetRandomPlayerUnit());        //temporary!!
        EndTurn();
    }

    public void EndTurn()
    {
        CombatManager.Instance.NextTurn();
    }
    
}
