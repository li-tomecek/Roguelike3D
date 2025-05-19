using System.Collections.Generic;
using UnityEngine;


public struct Stats
{
    public int maxHealth;
    public int attack, defense, agility;
}

public class UnitStatus : MonoBehaviour
{
    //Stats
    [SerializeField] private Stats _stats;
    private Stats _modifiers;
    private int _health;

    //Skills
    private List<Skill> _skills = new List<Skill>();
    private Skill _defaultSkill;
    
    //Effects
    //private List<Effect> _activeEffects = new List<Effect>();

}
