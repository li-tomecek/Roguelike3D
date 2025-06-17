using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Scriptable Objects/Skill/AttackSkill")]
public class AttackSkill : Skill
{
    [FormerlySerializedAs("_damage")]
    [Header("Combat Information")]
    [SerializeField] int damage;
    [SerializeField] private bool useAttackStat;
    [SerializeField] private bool ignoresDefense;
    //[SerializeField] int _accuracy;
    
    public override void UseSkill(Unit caster, Unit target)
    {
        int dmg = damage;
        
        if (useAttackStat)
            dmg += caster.GetStats().attack;
 
        if(!ignoresDefense)
            dmg = Mathf.Max(dmg - target.GetStats().defense, 0);
        
        //check accuracy

        //apply damage (or healing)
        target.TakeDamage(dmg);

        CombatInterface.Instance.SetDamageIndicator(dmg, target.gameObject.transform);
        Debug.Log(target.gameObject.name + " took " + dmg + " damage!");

        base.UseSkill(caster, target);
    }
}
