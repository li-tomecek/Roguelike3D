
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

public class PlayerTurnManager : TurnManager
{
    private PlayerUnit unit;
    private Skill _activeSkill;
    private int _targetIndex;
    private List<Unit> _targetPool;
    public PlayerTurnManager(PlayerUnit unit) : base()
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();
        CombatManager.Instance.GetCombatMenu().SetupCombatMenu(unit);
    }

    public override void EndTurn()
    {
        InputController.Instance.SubmitEvent.RemoveAllListeners();
        InputController.Instance.NavigateEvent.RemoveAllListeners();
        base.EndTurn();
    }
    
    // --- Player-specific Methods ---
    // -------------------------------
    private void ConfirmTarget()
    {
        //this is where you actually use the skill
        _activeSkill.UseSkill(unit, CombatManager.Instance.GetEnemyUnits()[_targetIndex]);
       
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

    public void ChooseTargetForSkill(Skill skill)
    {
        _targetIndex = 0;
        _activeSkill = skill;
        
        /*switch (skill.GetTargetMode())
        {
            case TargetMode.MELEE:
                _targetPool = CombatManager.Instance.GetEnemyUnits();
                CombatManager.Instance.SetTargetArrowPositionAtEnemy(_targetIndex);     //Todo: make sure the enemy melee targets are not actually ranged
                break;
            case TargetMode.ALL:
                break;
            case TargetMode.RANGED:
                _targetPool = CombatManager.Instance.GetEnemyUnits();
                CombatManager.Instance.SetTargetArrowPositionAtEnemy(_targetIndex);     //Todo: make sure the enemy melee targets are actually ranged
                break;
            case TargetMode.ALLY:
                _targetPool = CombatManager.Instance.GetPlayerUnits();
                CombatManager.Instance.SetTargetArrowPosotionAtAlly(_targetIndex);     //Todo: make sure the enemy melee targets are actually ranged
                break;
        }*/
        
        CombatManager.Instance.SetTargetArrowPositionAtEnemy(_targetIndex);
        
        //these must be the last statements in the function
        InputController.Instance.SubmitEvent.AddListener(ConfirmTarget); 
        InputController.Instance.NavigateEvent.AddListener(CycleTarget);
    }

}
