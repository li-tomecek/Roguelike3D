using System;
using System.Collections.Generic;
using UnityEngine;

public enum TargetMode
{
    MELEE, RANGED, ALLY, ALL_ENEMIES, ALL_ALLIES
}
public class Skill : ScriptableObject
{
    [Header("Skill Information")]
    [SerializeField] public new string name;
    [SerializeField] public string description;
    
    [SerializeField] protected int cost;
    [SerializeField] protected TargetMode targetMode;
    
    //public Effect effect
    [SerializeField] protected List<Skill> upgrades = new List<Skill>();    //possible upgrade paths for a certain skill.
   
    public virtual void UseSkill(Unit caster, Unit target)
    {
        //Spend BP
        caster.DecrementBP(cost);
    }

    public virtual float CalculateSkillPriority(EnemyUnit caster, Unit target)      //for enemy AI, to implement separately for each type of move.
    {
        return 0f;
        //default implementation just returns 0.
    }
    
    
    
    public TargetMode GetTargetMode() { return targetMode; }
    public int GetCost() { return cost; }
    public List<Skill> GetUpgrades() { return upgrades; }
}


