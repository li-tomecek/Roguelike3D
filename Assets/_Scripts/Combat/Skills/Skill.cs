using System;
using System.Collections.Generic;
using UnityEngine;

public enum TargetMode
{
    MELEE, RANGED, ALLY, ALL
}

//[CreateAssetMenu(fileName = "Skill", menuName = "Scriptable Objects/Skill")]
public class Skill : ScriptableObject
{
    [Header("Skill Information")]
    [SerializeField] protected string name;
    [SerializeField] protected string description;
    
    [SerializeField] protected int cost;
    //[SerializeField] protected TargetMode targetMode;
    //public Effect effect
    
    //public List<Skill> upgrades = new List<Skill> ();
   
    public virtual void UseSkill(Unit target)
    {
        Debug.Log("Performed " + name);
    }

}


