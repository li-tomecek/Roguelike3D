using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
}

public class Unit : MonoBehaviour
{
    [FormerlySerializedAs("_stats")]
    [Header("Combat")]
    //Stats
    [SerializeField] protected Stats stats; 
    [SerializeField] protected Stats modifiers; 
    protected int _health;

    //Skills
    [SerializeField] protected List<Skill> skills = new List<Skill>();
    [SerializeField] protected Skill defaultSkill;
    
    //Effects
    //private List<Effect> _activeEffects = new List<Effect>();
    
    protected TurnManager turnManager;
    
    //----------------------------------------------------
    //---------------------------------------------------

    protected virtual void Start()
    {
        _health = stats.maxHealth;
    }
    
    
    // --- Combat Methods ---
    public void UseDefaultSkill(Unit target)
    {
        defaultSkill.UseSkill(target);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = _health < 0 ? 0 : _health;

        if (_health <= 0)
        {
            CombatManager.Instance.RemoveFromCombat(this);
            Debug.Log($"{name} is Dead.");
            //Destroy(this.gameObject);   //temp
        }
    }
    
    
    
    // --- Getters / Setters ---
    public Stats GetStats() { return stats; }
    public virtual TurnManager GetTurnManager() { return turnManager; }

}
