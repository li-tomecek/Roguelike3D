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
    public string name;
    public string description;
    
    public int cost;
    public TargetMode targetMode;

    public int strength;            //how "effective" this skill is
    public float accuracy;          // btwn 0 and 1
    
    //public List<Skill> upgrades = new List<Skill> ();
    //public Effect effect
    private void Perform(UnitStatus target)
    {
        //check accuracy

        //apply damage (or healing)

        //apply effect
        
        //apply cost

        Debug.Log("Performed " + name);
    }

}


