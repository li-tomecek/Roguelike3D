using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatReward : MonoBehaviour, IInteractable
{
    private List<Tuple<Skill, Skill>> _skillChoices = new List<Tuple<Skill, Skill>>();    //first represents upgrade, second represents the skill to be upgraded
    [SerializeField] private GameObject _rewardMenu;
    private List<RewardOption> optionPanels;

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

        List<Tuple<Skill, Skill>> upgrades = new List<Tuple<Skill, Skill>>();

        //1. Get all possible skill upgrades
        foreach (Skill skill in LevelManager.Instance.GetRewardedUnit().GetSkills())
        {
            foreach (Skill upgrade in skill.GetUpgrades())
            {
                upgrades.Add(Tuple.Create(upgrade, skill));
            }
        }

        //2. Pull two unique upgrades from the list
        Tuple<Skill, Skill> choice;
        for(int i = 0; i < CHOICE_AMT; i++)
        {
            do
            {
                choice = upgrades[UnityEngine.Random.Range(0, upgrades.Count)];
            } while (_skillChoices.Count > 0 && _skillChoices.Contains(choice));

            _skillChoices.Add(choice);
        }

        //3. Update the panels with the relevant information
        for(int i = 0; i < CHOICE_AMT; i++)
        {
            optionPanels[i].SetupPanelForSkill(_skillChoices[i]);
        }

    }

    public void CloseMenu()
    {
        InputController.Instance.ActivateMovementMap();
        _rewardMenu.SetActive(false);
    }
}
