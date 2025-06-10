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
    [Header("Combat")]
    //Stats
    [SerializeField] protected Stats stats; 
    [SerializeField] protected Stats modifiers; 
    protected int _health;

    //Skills
    [SerializeField] protected List<Skill> skills;
    [SerializeField] protected Skill defaultSkill;


    //Effects
    //private List<Effect> _activeEffects = new List<Effect>();

    protected TurnManager turnManager;
    protected HealthBar healthBar;
    
    //----------------------------------------------------
    //---------------------------------------------------

    protected virtual void Start()
    {
        _health = stats.maxHealth;
        healthBar = gameObject.GetComponentInChildren<HealthBar>();

        healthBar.gameObject.SetActive(false);  //hide health bar until combat
    }
    
    
    // --- Combat Methods ---
    public void UseDefaultSkill(Unit target)
    {
        defaultSkill.UseSkill(this, target);
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        _health = _health < 0 ? 0 : _health;
        healthBar.SetSliderPercent((float)_health / stats.maxHealth);

        if (_health <= 0)
        {
            CombatManager.Instance.RemoveFromCombat(this);
            Debug.Log($"{name} is Dead.");
            
            this.gameObject.SetActive(false);   //temp
        }
    }
    
    
    
    // --- Getters / Setters ---
    public Stats GetStats() { return stats; }
    public virtual TurnManager GetTurnManager() { return turnManager; }
    public Skill GetDefaultSkill() { return defaultSkill; }
    public List<Skill> GetSkills() { return skills; }

    public HealthBar GetHealthBar() { return healthBar; }
}
