
using System.Collections.Generic;
using UnityEngine;
//using System.Diagnostics;

public class PlayerTurnManager : TurnManager
{
    private PlayerUnit unit;
    private Skill _activeSkill;
    private int _targetIndex;
    private List<Unit> _targetPool;
    public PlayerTurnManager(PlayerUnit unit) : base(unit)
    {
        this.unit = unit;
    }
    
    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();
        //unit.IncrementBP();
        CombatInterface.Instance.GetTurnMenu().SetupMenu(unit);
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
        _activeSkill.UseSkill(unit, _targetPool[_targetIndex]);

        CombatInterface.Instance.HideArrow();
        EndTurn();
    }

    private void CycleTarget(Vector2 input)
    {
        //we are assuming targeting is only for enemies for now 

        if (input.x < 0 || input.y < 0)
        {
            //selection moves left
            _targetIndex = (_targetIndex == 0) ? _targetPool.Count - 1 : (_targetIndex - 1);
        }
        else if (input.x > 0 || input.y > 0)
        {
            _targetIndex = (_targetIndex == _targetPool.Count - 1) ? 0 : (_targetIndex + 1);
        }
        else
            return;

        CombatInterface.Instance.SetTargetArrowPosition(_targetPool[_targetIndex].transform.position);
    }

    public void ChooseTargetForSkill(Skill skill)
    {
        _targetIndex = 0;
        _activeSkill = skill;

        
        switch (skill.GetTargetMode())
        {

            case TargetMode.RANGED:
                _targetPool = CombatManager.Instance.GetEnemyUnits();
                break;
            case TargetMode.MELEE:
                _targetPool = CombatManager.Instance.GetEnemyUnits();
                break;
            case TargetMode.ALL_ENEMIES:
                _targetPool = CombatManager.Instance.GetEnemyUnits();
                break;
            case TargetMode.ALLY:
                _targetPool = CombatManager.Instance.GetPlayerUnits();
                break;
            case TargetMode.ALL_ALLIES:
                _targetPool = CombatManager.Instance.GetPlayerUnits();
                break;
          
            //ToDo: Setup proper targeting for 'RANGED', and 'MELEE'
        }

        //a)  Use skill on all applicable targets
        if(skill.GetTargetMode() == TargetMode.ALL_ENEMIES || skill.GetTargetMode() == TargetMode.ALL_ALLIES)
        {
            foreach(Unit target in _targetPool)
            {
                _activeSkill.UseSkill(unit, target);
            }
            EndTurn();
        }
        //b) Setup targeting arrow for target selection
        else
        {
            CombatInterface.Instance.SetTargetArrowPosition(_targetPool[_targetIndex].transform.position);

            //these must be the last statements in the function
            InputController.Instance.SubmitEvent.AddListener(ConfirmTarget);
            InputController.Instance.NavigateEvent.AddListener(CycleTarget);
        }
    }

}
