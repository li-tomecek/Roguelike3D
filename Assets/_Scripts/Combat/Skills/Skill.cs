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
    [SerializeField] public string description;
    
    [SerializeField] protected int cost;
    [SerializeField] protected TargetMode targetMode;
    
    //public Effect effect
    [SerializeField] protected List<Skill> upgrades = new List<Skill>();    //possible upgrade paths for a certain skill.
   
    public virtual void UseSkill(Unit caster, Unit target)
    {
        //1. Spend BP
        caster.DecrementBP(cost);

        //2. Look at Target
        Quaternion originalRotation = caster.transform.rotation;
        if (targetMode == TargetMode.MELEE || targetMode == TargetMode.RANGED)
            caster.gameObject.transform.LookAt(target.transform, Vector3.up);

        //3. Use Skill animation
        switch (targetMode)
        {
            case TargetMode.MELEE:
                //Run towards target here
                if(caster.gameObject.GetComponent<Animator>())
                    caster.gameObject.GetComponent<Animator>().SetTrigger("MeleeAttack");   //Todo: add sequence where caster runs towards target 
                break;
            
            default:
                if (caster.gameObject.GetComponent<Animator>())
                    caster.gameObject.GetComponent<Animator>().SetTrigger("MeleeAttack");   //Todo: replace with ranged attack 
                break;


        }
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


