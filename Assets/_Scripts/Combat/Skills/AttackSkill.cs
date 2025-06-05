using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Scriptable Objects/Skill/AttackSkill")]
public class AttackSkill : Skill
{
    [FormerlySerializedAs("_damage")]
    [Header("Combat Information")]
    [SerializeField] int damage;
    [SerializeField] private bool useAttackStat;
    //[SerializeField] int _accuracy;
    
    public override void UseSkill(Unit caster, Unit target)
    {
        if (useAttackStat)
            damage += caster.GetStats().attack;
        
        damage = Mathf.Max(damage - target.GetStats().defense, 0);
        
        //check accuracy

        //apply damage (or healing)
        target.TakeDamage(damage);
        
        //apply effect
        
        //apply cost

        Debug.Log(target.gameObject.name + " took " + damage + " damage!");
    }
}
