using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CombatReward : MonoBehaviour, IInteractable
{
    private List<Tuple<Skill, Skill>> _skillChoices;    //first represents upgrade, second represents the skill to be upgraded
    
    public void Interact()
    {
        //ToDo: display menu with upgrade options
    }

    public void SetupCharacterRewards(PlayerUnit unit)
    {

        List<Tuple<Skill, Skill>> upgrades = new List<Tuple<Skill, Skill>>();

        //1. Get all possible skill upgrades
        foreach (Skill skill in unit.GetSkills())
        {
            foreach (Skill upgrade in skill.GetUpgrades())
            {
                upgrades.Add(Tuple.Create(upgrade, skill));
            }
        }

        //2. Pull two unique upgrades from the list
        Tuple<Skill, Skill> choice;
        for(int i = 0; i < 2; i++)
        {
            do
            {
                choice = upgrades[UnityEngine.Random.Range(0, upgrades.Count)];
            } while (_skillChoices.Contains(choice));

            _skillChoices.Add(choice);
        }

    }
}
