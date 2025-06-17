using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "HealSkill", menuName = "Scriptable Objects/Skill/HealSkill")]
public class HealSkill : Skill
{
    [Header("Combat Information")]
    [SerializeField] int _healAmt;
    [SerializeField] private bool asPercentage;    
    public override void UseSkill(Unit caster, Unit target)
    {
        int heal = _healAmt;
        
        if (asPercentage)   // we treat this as a percentage from 0 to 100
        {
            heal = (int) (Mathf.Min((_healAmt / 100f), 1f) * target.GetStats().maxHealth);
        }

        heal = Mathf.Min(heal, target.GetStats().maxHealth - target.GetHealth());   //cannot heal for over 100 HP

        //apply  healing
        target.TakeDamage(-heal);

        CombatInterface.Instance.SetDamageIndicator(heal, target.gameObject.transform, UnityEngine.Color.green);
        Debug.Log(target.gameObject.name + " restored " + heal + " damage!");

        base.UseSkill(caster, target);
    }
}
