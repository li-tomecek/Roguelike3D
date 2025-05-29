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
        //ToDo: resolve any active effects

        //this must be the last statement in the function
        InputController.Instance.ReAssignSubmitEvent(UseDefaultSkill);  //temporary! when the player confirms, the unit will use their default skill
        
    }

    public void EndTurn()
    {
        CombatManager.Instance.NextTurn();
    }
    
    public void UseDefaultSkill()
    {
        _unit.UseDefaultSkill(CombatManager.Instance.GetRandomPlayerUnit());
        EndTurn();
    }
    
}
