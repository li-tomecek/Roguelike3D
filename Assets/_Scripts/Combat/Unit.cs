using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
}

public class Unit : MonoBehaviour
{
    [Header("Combat")]
    //Stats
    [SerializeField] protected Stats _stats; 
    [SerializeField] protected Stats _modifiers; 
    protected int _health;

    //Skills
    [SerializeField] protected List<Skill> _skills = new List<Skill>();
    [SerializeField] protected Skill _defaultSkill;
    
    //Effects
    //private List<Effect> _activeEffects = new List<Effect>();
    
    private TurnManager _turnManager;
    
    //----------------------------------------------------
    //---------------------------------------------------

    private void Start()
    {
        _turnManager = new TurnManager(this);
        _health = _stats.maxHealth;
    }
    
    
    
    // --- Combat Methods ---
    public void UseDefaultSkill(Unit target)
    {
        _defaultSkill.UseSkill(target);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = _health < 0 ? 0 : _health;

        if (_health <= 0)
        {
            CombatManager.Instance.RemoveFromCombat(this);
        }
    }
    
    
    
    // --- Getters / Setters ---
    public Stats GetStats() { return _stats; }
    public TurnManager GetTurnManager() { return _turnManager; }

}
