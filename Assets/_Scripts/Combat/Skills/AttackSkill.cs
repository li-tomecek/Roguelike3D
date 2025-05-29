using UnityEngine;

[CreateAssetMenu(fileName = "AttackSkill", menuName = "Scriptable Objects/Skill/AttackSkill")]
public class AttackSkill : Skill
{
    [Header("Combat Information")]
    [SerializeField] int _damage;
    //[SerializeField] int _accuracy;
    
    public override void UseSkill(Unit target)
    {
        //check accuracy

        //apply damage (or healing)
        target.TakeDamage(_damage);
        
        //apply effect
        
        //apply cost

        Debug.Log(target.gameObject.name + " took " + _damage + " damage!");
    }
}
