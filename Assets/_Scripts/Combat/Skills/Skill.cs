using UnityEngine;

public enum TargetMode
{
    MELEE, RANGED, ALLY, ALL
}
public class Skill : ScriptableObject
{
    [Header("Skill Information")]
    [SerializeField] protected new string name;
    [SerializeField] protected string description;
    
    [SerializeField] protected int cost;
    [SerializeField] protected TargetMode targetMode;
    
    //public Effect effect
    //public List<Skill> upgrades = new List<Skill> ();
   
    public virtual void UseSkill(Unit caster, Unit target)
    {
        caster.DecrementBP(cost);
    }

    public TargetMode GetTargetMode() { return targetMode; }

    public int GetCost() { return cost; }
}


