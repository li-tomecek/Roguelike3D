
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

public class PlayerTurnManager : TurnManager
{
    private Skill _activeSkill;
    private int _targetIndex;
    public PlayerTurnManager(Unit unit) : base(unit)
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    // --------------------------
    public override void StartTurn()
    {
        base.StartTurn();
        _targetIndex = 0;
        _activeSkill = unit.GetDefaultSkill();

        CombatManager.Instance.SetTargetArrowPositionAtEnemy(_targetIndex);
        
        //these must be the last statements in the function
        InputController.Instance.SubmitEvent.AddListener(ConfirmTarget);  //temporary! when the player confirms, the unit will use their default skill
        InputController.Instance.NavigateEvent.AddListener(CycleTarget);
    }

    public override void EndTurn()
    {
        InputController.Instance.SubmitEvent.RemoveAllListeners();
        InputController.Instance.NavigateEvent.RemoveAllListeners();
        base.EndTurn();
    }
    
    // --- Player-specific Methods ---
    // -------------------------------
    public void UseDefaultSkill()
    {
        unit.UseDefaultSkill(CombatManager.Instance.GetRandomEnemyUnit());
        EndTurn();
    }

    private void ConfirmTarget()
    {
        //this is where you actually use the skill
        _activeSkill.UseSkill(CombatManager.Instance.GetEnemyUnits()[_targetIndex]);
       
        CombatManager.Instance.HideArrow();
        EndTurn();

    }

    private void CycleTarget(Vector2 input)
    {
        //we are assuming targeting is only for enemies for now 

        if (input.x < 0 || input.y < 0)
        {
            //selection moves left
            _targetIndex = (_targetIndex == 0) ? CombatManager.Instance.GetEnemyUnits().Count - 1 : (_targetIndex - 1);
        }
        else if (input.x > 0 || input.y > 0)
        {
            _targetIndex = (_targetIndex == CombatManager.Instance.GetEnemyUnits().Count - 1) ? 0 : (_targetIndex + 1);
        }
        else
            return;

        CombatManager.Instance.SetTargetArrowPositionAtEnemy(_targetIndex);
    }

}
