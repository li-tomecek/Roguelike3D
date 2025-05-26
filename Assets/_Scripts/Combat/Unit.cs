using System.Collections.Generic;
using UnityEngine;


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
    protected Stats _modifiers;
    protected int _health;

    //Skills
    protected List<Skill> _skills = new List<Skill>();
    protected Skill _defaultSkill;
    
    //Effects
    //private List<Effect> _activeEffects = new List<Effect>();
    
    private TurnManager _turnManager;
    
    //----------------------------------------------------
    //---------------------------------------------------

    private void Start()
    {
        _turnManager = new TurnManager(this);
    }
    
    
    
    // --- Combat Methods ---
    public void UseDefaultSkill()
    {
        _defaultSkill.UseSkill(this);
    }
    
    
    
    // --- Getters / Setters ---
    public Stats GetStats() { return _stats; }
    public TurnManager GetTurnManager() { return _turnManager; }

}
