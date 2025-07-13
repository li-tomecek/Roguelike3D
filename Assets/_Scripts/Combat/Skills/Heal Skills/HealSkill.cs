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

        CombatInterface.Instance.SetIndicator(heal.ToString(), target.gameObject.transform, UnityEngine.Color.green);
        Debug.Log(target.gameObject.name + " restored " + heal + " damage!");

        base.UseSkill(caster, target);
    }

    public override float CalculateSkillPriority(EnemyUnit caster, Unit target)
    {
        float percentHP = (float)(target.GetHealth()) / target.GetStats().maxHealth;
        float score;

        if (percentHP > 0.5f)
            score = (1 - percentHP);                         // HP Above 50%: score scales linearly with how little HP target has remaining
        
        else
            score = (1 - (2 * Mathf.Pow(percentHP, 2f)));    // HP <= 50%: score scales quadratically with how little HP target has remaining

        return caster.C_Heal * score;
    }
}
