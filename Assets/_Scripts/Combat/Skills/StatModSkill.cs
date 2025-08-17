using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "StatModSkill", menuName = "Scriptable Objects/Skill/StatModSkill")]
public class StatModSkill : Skill
{
    [Header("Combat Information")]
    [SerializeField] int amount;
    [SerializeField] int duration;
    [SerializeField] StatType type;
    [SerializeField] bool isDebuff;

    [Header("Skill Priority")]
    [SerializeField][Range(0.0f, 1.0f)] private float _minBase = 0.3f;
    [SerializeField][Range(0.0f, 1.0f)] private float _maxBase = 0.7f;

    public override void UseSkill(Unit caster, Unit target)
    {
        
        Effect statModifier = new Effect(isDebuff ? -amount : amount, type, duration);
        statModifier.ApplyEffect(target);
        target.GetActiveEffects().Add(statModifier);
      

        if (isDebuff)
            CombatInterface.Instance.SetIndicator($"{type.ToString()}vv", target.gameObject.transform, new UnityEngine.Color(128f, 0f, 128f, 1f));  //purple
        else
            CombatInterface.Instance.SetIndicator($"{type.ToString()}^^", target.gameObject.transform, UnityEngine.Color.blue);
        
        Debug.Log($"{target.gameObject.name}'s {type.ToString()} was " + (isDebuff ? "de" : "") + $"buffed for {duration} turns!");
        base.UseSkill(caster, target);
    }

    public override float CalculateSkillPriority(EnemyUnit caster, Unit target)
    {
        float percentHP = (float)(target.GetHealth()) / target.GetStats().maxHealth;
        return caster.C_StatMod * (_minBase + percentHP * (_maxBase - _minBase));
    }
}
