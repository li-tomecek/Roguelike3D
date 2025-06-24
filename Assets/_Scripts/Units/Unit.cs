using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
}

public abstract class Unit : MonoBehaviour
{
    [Header("Combat")]
    //Stats
    [SerializeField] protected Stats stats; 
    [SerializeField] protected Stats modifiers; 
    protected int _health;
    protected int _bp;

    //Skills
    [SerializeField] protected List<Skill> skills;
    [SerializeField] protected Skill defaultSkill;


    //Effects
    private List<Effect> _activeEffects = new List<Effect>();

    protected TurnManager turnManager;
    protected HealthBar healthBar;
    
    //---------------------------------------------------
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
           
        }
    }

    public void ApplyModifier(EffectType type, int value)
    {
        switch (type)
        {
            case EffectType.ATK:
                modifiers.attack += value;
                break;
            case EffectType.DEF:
                modifiers.defense += value;
                break;
            case EffectType.AGI:
                modifiers.agility += value;
                break;

        }
    }

    public void ResolveEffects()
    {
        for(int i = 0; i < _activeEffects.Count; i++)
        {
            _activeEffects[i].duration--;
        }
    }


    // --- Getters / Setters ---
    public Stats GetStats() { return stats; }
    public Stats GetModifiers() { return modifiers; }
    public List<Effect> GetActiveEffects() { return _activeEffects; }
    public virtual TurnManager GetTurnManager() { return turnManager; }
    public Skill GetDefaultSkill() { return defaultSkill; }
    public List<Skill> GetSkills() { return skills; }
    public HealthBar GetHealthBar() { return healthBar; }
    public int GetHealth() { return _health; }
    public void SetHealth(int value) { _health = value; }
    public int GetBP() { return _bp; }
    
    public void IncrementBP() { _bp++; }
    public void DecrementBP(int amt) { _bp = Mathf.Max(_bp -amt, 0); }
}
