using System;
using System.Collections.Generic;
using UnityEngine;

public enum TargetMode
{
    MELEE, RANGED, ALLY, ALL
}

[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    protected string name;
    protected string description;
    
    protected int cost;
    protected TargetMode targetMode;
    
    //public List<Skill> upgrades = new List<Skill> ();
    //public Effect effect
    public virtual void UseSkill(Unit target)
    {
        //check accuracy

        //apply damage (or healing)

        //apply effect
        
        //apply cost

        Debug.Log("Performed " + name);
    }

}


