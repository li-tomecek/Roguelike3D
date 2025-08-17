using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Scriptable Objects/Skill/AttackSkill")]
public class AttackSkill : Skill
{
    [Header("Combat Information")]
    [SerializeField] int damage;
    [SerializeField] private bool useAttackStat;
    [SerializeField] private bool ignoresDefense;

    [Header("Skill Priority")]
    [SerializeField][Range(0.0f, 1.0f)] private float _hpThreshold = 0.7f;
    [SerializeField][Range(0.0f, 1.0f)] private float _minScore = 0.2f;
    
    public override void UseSkill(Unit caster, Unit target)
    {
        int dmg = CalculateTotalDamage(caster, target);

        target.TakeDamage(dmg);

        base.UseSkill(caster, target);
    }

    public override float CalculateSkillPriority(EnemyUnit caster, Unit target)
    {
        int dmg = CalculateTotalDamage(caster, target);
        float percentDamagedHP = Mathf.Max((float)(target.GetHealth() - dmg) / target.GetStats().maxHealth, 0f);   //taking amount of damage dealt into account
        float score;

        if (percentDamagedHP >= _hpThreshold)
            score = _minScore;
        else
            score = _minScore + (_hpThreshold - percentDamagedHP) * ((1 - _minScore) / (_hpThreshold));         //linear increase from minScore to 1, as HP remaining decreases.
                                                                                                            

            return caster.C_Attack * score;
    }

    public int CalculateTotalDamage(Unit caster, Unit target)
    {
        int dmg = damage;

        if (useAttackStat)
            dmg += (caster.GetStats().attack + caster.GetModifiers().attack);

        if (!ignoresDefense)
            dmg = Mathf.Max(dmg - (target.GetStats().defense + target.GetModifiers().defense), 0);
        
        return dmg;
    }
}
