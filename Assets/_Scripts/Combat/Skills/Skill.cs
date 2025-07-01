using System;
using System.Collections.Generic;
using UnityEngine;

public enum TargetMode
{
    MELEE, RANGED, ALLY, ALL_ENEMIES, ALL_ALLIES, ANY_ENEMY
}
public class Skill : ScriptableObject
{
    [Header("Skill Information")]
    [SerializeField] public new string name;
    [SerializeField] protected string description;
    
    [SerializeField] protected int cost;
    [SerializeField] protected TargetMode targetMode;
    
    //public Effect effect
    [SerializeField] protected List<Skill> upgrades = new List<Skill>();    //possible upgrade paths for a certain skill.
   
    public virtual void UseSkill(Unit caster, Unit target)
    {
        caster.DecrementBP(cost);
    }

    public virtual Tuple<Unit, float> getOptimalTarget(List<Unit> targets)      //for enemy AI, to implement separately for each type of move.
    {
        return Tuple.Create(targets[UnityEngine.Random.Range(0, targets.Count)], 0f);
        //default implementation just returns a random target.
    }
    
    
    
    public TargetMode GetTargetMode() { return targetMode; }
    public int GetCost() { return cost; }
    public List<Skill> GetUpgrades() { return upgrades; }
}


