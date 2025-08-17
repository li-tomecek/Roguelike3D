using System;
using System.Globalization;
using System.Text;
using TMPro;
using UnityEngine;

public class RewardOption : MonoBehaviour
{
    //private Tuple<Skill, Skill> _associatedSkills;
    private Upgrade upgrade;

    [SerializeField] TextMeshProUGUI _oldNameText;
    [SerializeField] TextMeshProUGUI _newNameText;
    [SerializeField] TextMeshProUGUI _costText;
    [SerializeField] TextMeshProUGUI _descriptionText;

    public void SetupPanelForUpgrade(Upgrade up)
    {
        upgrade = up;

        if(up is SkillUpgrade)
        {
            SkillUpgrade skillUpgrade = (SkillUpgrade)up;
            SetupPanelForSkillUp(skillUpgrade);
        } 
        else if(up is StatUpgrade)
        {
            StatUpgrade levelUpgrade = (StatUpgrade)up;
            SetupPanelForStatUp(levelUpgrade);
        }
    }
    
    private void SetupPanelForSkillUp(SkillUpgrade skUp)
    {
        _oldNameText.SetText(skUp.skillUpgrade.Item1.name);
        _newNameText.SetText(skUp.skillUpgrade.Item2.name);
        
        _costText.SetText("BP: " + skUp.skillUpgrade.Item2.GetCost());
        _descriptionText.SetText(skUp.skillUpgrade.Item2.description);
    }

    private void SetupPanelForStatUp(StatUpgrade statUp)
    {

        Stats oldStats = statUp.statUpgrade.Item1;
        Stats newStats = statUp.statUpgrade.Item2;

        _oldNameText.SetText($"Lvl {oldStats.level}");
        _newNameText.SetText($"Lvl {newStats.level}");

        StringBuilder builder = new StringBuilder();
        string hpTxt = (oldStats.maxHealth != newStats.maxHealth) ? $" >> {newStats.maxHealth}" : "";
        string atkTxt = (oldStats.attack != newStats.attack) ? $" >> {newStats.attack}" : "";
        string defTxt = (oldStats.defense != newStats.defense) ? $" >> {newStats.defense}" : "";
        string agiTxt = (oldStats.agility != newStats.agility) ? $" >> {newStats.agility}" : "";
        
        builder.AppendFormat($"HP: {oldStats.maxHealth}{hpTxt}\nATK: {oldStats.attack}{atkTxt}\nDEF: {oldStats.defense}{defTxt}\nAGI: {oldStats.agility}{agiTxt}\n");
        
        _costText.SetText(builder);
        _descriptionText.SetText("");
    }

    public void ChooseThisUpgrade()
    {
        ////LevelManager.Instance.GetRewardedUnit().ReplaceSkill(_associatedSkills.Item2, _associatedSkills.Item1);
        ///LevelManager.Instance.ClaimReward();
        if (upgrade is SkillUpgrade)
        {
            SkillUpgrade skUp = (SkillUpgrade)upgrade;
            LevelManager.Instance.GetRewardedUnit().ReplaceSkill(skUp.skillUpgrade.Item1, skUp.skillUpgrade.Item2);
        }
        else if (upgrade is StatUpgrade)
        {
            StatUpgrade stUp = (StatUpgrade)upgrade;
            LevelManager.Instance.GetRewardedUnit().SetStats(stUp.statUpgrade.Item2);
        }

        LevelManager.Instance.ClaimReward();
    }
}
