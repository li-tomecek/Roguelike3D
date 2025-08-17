
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using System.Diagnostics;

public class PlayerTurnManager : TurnManager
{
    public void Start()
    {
        this.unit = GetComponent<PlayerUnit>();
    }

    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();
        CombatInterface.Instance.GetTurnMenu().SetupMenu((PlayerUnit)unit);
        unit.SetGuard(false);       //should put this as part of TurnStartEvent
        ((PlayerUnit)unit).OnTurnStart.Invoke();
    }

    public override void EndTurn()
    {
        InputController.Instance.SubmitEvent.RemoveAllListeners();
        InputController.Instance.NavigateEvent.RemoveAllListeners();
        ((PlayerUnit)unit).OnTurnEnd.Invoke();
        base.EndTurn();
    }
    
    // --- Player-specific Methods ---
    // -------------------------------
    private void ConfirmTarget()
    {
        CombatInterface.Instance.HideArrow();
        StartCoroutine(PlayTurnSequence(_activeSkill, _targetPool[_targetIndex]));
    }

    private void CycleTarget(Vector2 input)
    {
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
        InputController.Instance.CancelEvent.AddListener(ReOpenTurnMenu);

        _targetIndex = 0;
        _activeSkill = skill;

        SetupTargetPool(true);

        //a)  Use skill on all applicable targets
        if (skill.GetTargetMode() == TargetMode.ALL_ENEMIES || skill.GetTargetMode() == TargetMode.ALL_ALLIES)
        {
            StartCoroutine(PlayTurnSequence(skill, _targetPool[0]));    // target doesn't matter here anyways
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

    private void ReOpenTurnMenu()
    {
        Debug.Log($"Repoening menu for {unit.name}; it is {CombatManager.Instance.GetUnitWithCurrentTurn().name}'s turn.");
        CombatInterface.Instance.HideArrow();
        InputController.Instance.SubmitEvent.RemoveAllListeners();
        InputController.Instance.NavigateEvent.RemoveAllListeners();
        CombatInterface.Instance.GetTurnMenu().SetupMenu((PlayerUnit)unit);
    }
}
