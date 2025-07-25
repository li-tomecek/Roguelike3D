
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTurnManager : TurnManager
{

    private float highestScore;

    public void Start()
    {
        this.unit = GetComponent<EnemyUnit>();
    }

    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();

        //this must be the last statement in the function    
        ChooseSkillAndTarget();
    }

    private void ChooseSkillAndTarget()
    {
        highestScore = 0f;
        List<Tuple<float, Skill, Unit>> calculatedMoves = new List<Tuple<float, Skill, Unit>>();    //Lots of data, but there should be at most 9 possible target-skill combinations

        //1. Get all possible skill-target combination scores
        calculatedMoves.AddRange(GetPriorotyScoresForSkill(unit.GetDefaultSkill()));
        
        foreach (Skill skill in unit.GetSkills())
        {
            if (skill.GetCost() <= unit.GetBP())
                calculatedMoves.AddRange(GetPriorotyScoresForSkill(skill));
        }

        //2. Only Consider scores that meet the threshold.  ~ The smaller the threshold, the more skills are cosidered, and the "dumber" the AI is
        List<Tuple<float, Skill, Unit>> finalSelection = new List<Tuple<float, Skill, Unit>>();     //making a new list is less expensive then removing several values

        for (int i = 0; i < calculatedMoves.Count; i++)
        {

            if (calculatedMoves[i].Item1 >= highestScore * ((EnemyUnit)unit).MaxPriorityThreshold)
            {
                finalSelection.Add(calculatedMoves[i]);
            }
        }

        //3. Choose randomly from the final list
        int index = UnityEngine.Random.Range(0, finalSelection.Count);

        Debug.Log($"{unit.name} chose {finalSelection[index].Item2.name} on {finalSelection[index].Item3.name}! ({finalSelection[index].Item1} / {highestScore})");
        
        _activeSkill = finalSelection[index].Item2;
        SetupTargetPool(false);     //will have to re-calculate target pool
        
        StartCoroutine(PlayTurnSequence(finalSelection[index].Item2, finalSelection[index].Item3));
    }

    private List<Tuple<float, Skill, Unit>> GetPriorotyScoresForSkill(Skill skill)
    {
        List<Tuple<float, Skill, Unit>> tuples = new List<Tuple<float, Skill, Unit>>();

        _activeSkill = skill;
        SetupTargetPool(false);
        float score;

        if (skill.GetTargetMode() == TargetMode.ALL_ENEMIES || skill.GetTargetMode() == TargetMode.ALL_ALLIES)
        {
            //use a random target within the pool to judge skill score
            score = skill.CalculateSkillPriority((EnemyUnit)unit, _targetPool[UnityEngine.Random.Range(0, _targetPool.Count)]);
            tuples.Add(Tuple.Create(score, skill, unit));  // in this case, the target given for the tuple should not matter. Passing reference to self so that any miscalculations are obvious

            highestScore = Mathf.Max(highestScore, score);
        }
        else
        {
            foreach(Unit target in _targetPool)
            {
                score = skill.CalculateSkillPriority((EnemyUnit) unit, target);
                tuples.Add(Tuple.Create(score, skill, target));
                
                highestScore = Mathf.Max(highestScore, score); 
            }
        }

        return tuples;
    }
}
