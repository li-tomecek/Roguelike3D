using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Scriptable Objects/Skill/AttackSkill")]
public class AttackSkill : Skill
{
    [Header("Combat Information")]
    [SerializeField] int damage;
    [SerializeField] private bool useAttackStat;
    [SerializeField] private bool ignoresDefense;
    
    public override void UseSkill(Unit caster, Unit target)
    {
        int dmg = damage;
        
        if (useAttackStat)
            dmg += (caster.GetStats().attack + caster.GetStats().attack);
 
        if(!ignoresDefense)
            dmg = Mathf.Max(dmg - (target.GetStats().defense + target.GetStats().defense), 0);

        //apply damage (or healing)
        target.TakeDamage(dmg);

        CombatInterface.Instance.SetIndicator(dmg.ToString(), target.gameObject.transform);
        Debug.Log(target.gameObject.name + " took " + dmg + " damage!");

        base.UseSkill(caster, target);
    }
}
