using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatReward : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _rewardMenu;
    private List<RewardOption> optionPanels;

    private List<Upgrade> upgrades;
    private const float PERCENT_CHANCE_LEVELUP = 0.5f;
    private const int STAT_UPGRADE_AMOUNT = 2;
    List<Tuple<Skill, Skill>> possibleSkillUgrades;


    const int CHOICE_AMT = 2;

    public void Awake()
    {
        _rewardMenu = Instantiate(_rewardMenu);
        optionPanels = _rewardMenu.GetComponentsInChildren<RewardOption>().ToList();
        SetupCharacterRewards();
        _rewardMenu.SetActive(false);
    }

    public void Interact()
    {
        //Display menu with upgrade options
        InputController.Instance.ActivateMenuMap();
        _rewardMenu.SetActive(true);
    }

    public void SetupCharacterRewards()
    {

        //1. Get all possible skill upgrades
        possibleSkillUgrades = new List<Tuple<Skill, Skill>>();

        foreach (Skill skill in LevelManager.Instance.GetRewardedUnit().GetSkills())
        {
            foreach (Skill upgrade in skill.GetUpgrades())
            {
                possibleSkillUgrades.Add(Tuple.Create(skill, upgrade));
            }
        }

        //2. Decide how many upgrades are going to be skills, and how many will be level-ups
        int amount;
        if (possibleSkillUgrades.Count == 0)
        {
            amount = 0;
        }
        else
        {
            amount = 1 + (UnityEngine.Random.Range(0f, 1f) < PERCENT_CHANCE_LEVELUP ? 1 : 0);  //First will always be a skill if possible
        }

        //3. Choose the upgrades
        TrySetSkillUpgrades(amount);
        TrySetLevelUpgrades(amount);
        
 
        //4. Update the panels with the relevant information
        for (int i = 0; i < optionPanels.Count; i++)
        {
            optionPanels[i].SetupPanelForUpgrade(upgrades[i]);
        }

    }

    public void TrySetSkillUpgrades(int amount)
    {
        Tuple<Skill, Skill> choice;
        for (int i = 0; i < amount; i++)
        {
            if (possibleSkillUgrades.Count <= 0)        //if no possible skill upgrade, chose a level upgrade instead
                TrySetLevelUpgrades(1);
            
            int j = UnityEngine.Random.Range(0, possibleSkillUgrades.Count);
            
            choice = possibleSkillUgrades[j];
            possibleSkillUgrades.RemoveAt(j);           //probably not the most efficient way to do this

            SkillUpgrade newUpgrade = new SkillUpgrade(choice);
            upgrades.Add(newUpgrade);
        }
    }

    public void TrySetLevelUpgrades(int amount)
    {       
        int lastIndex = -1;
        int statIndex;
        
        for (int i = 0; i < amount; i++)
        {
            Stats upgradedStats = LevelManager.Instance.GetRewardedUnit().GetStats();

            do
            {
                statIndex = UnityEngine.Random.Range(0, Enum.GetValues(typeof (StatType)).Length);
            } while (statIndex == lastIndex);

            switch (statIndex)
            {
                case ((int)StatType.HP):
                    upgradedStats.maxHealth += STAT_UPGRADE_AMOUNT;
                    break;
                case ((int)StatType.ATK):
                    upgradedStats.attack += STAT_UPGRADE_AMOUNT;
                    break;
                case ((int)StatType.DEF):
                    upgradedStats.defense += STAT_UPGRADE_AMOUNT;
                    break;
                case ((int)StatType.AGI):
                    upgradedStats.agility += STAT_UPGRADE_AMOUNT;
                    break;

            }

            lastIndex = statIndex;

            StatUpgrade newUpgrade = new StatUpgrade(Tuple.Create(LevelManager.Instance.GetRewardedUnit().GetStats(), upgradedStats));
            upgrades.Add(newUpgrade);
        }
    }

    public void CloseMenu()
    {
        InputController.Instance.ActivateMovementMap();
        _rewardMenu.SetActive(false);
    }
}


public abstract class Upgrade{}

public class StatUpgrade : Upgrade
{
    public Tuple<Stats, Stats> statUpgrade;
    public StatUpgrade(Tuple<Stats, Stats> statUpgrade)
    {
        this.statUpgrade = statUpgrade;
    }
}

public class SkillUpgrade : Upgrade
{
    public Tuple<Skill, Skill> skillUpgrade;

    public SkillUpgrade(Tuple<Skill, Skill> skillUpgrade)
    {
        this.skillUpgrade = skillUpgrade;
    }
}
