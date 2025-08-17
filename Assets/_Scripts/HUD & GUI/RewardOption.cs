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

    private void SetupPanelForStatUp(StatUpgrade lvUp)
    {

        Stats oldStats = lvUp.statUpgrade.Item1;
        Stats newStats = lvUp.statUpgrade.Item2;

        _oldNameText.SetText($"Lvl {oldStats.level}");
        _newNameText.SetText($"Lvl {newStats.level}");

        StringBuilder builder = new StringBuilder();
        string hpTxt = (oldStats.maxHealth != newStats.maxHealth) ? $" >> {newStats.maxHealth}" : "";
        string atkTxt = (oldStats.attack != newStats.attack) ? $" >> {newStats.attack}" : "";
        string defTxt = (oldStats.defense != newStats.defense) ? $" >> {newStats.defense}" : "";
        string agiTxt = (oldStats.agility != newStats.agility) ? $" >> {newStats.agility}" : "";
        
        builder.AppendFormat($"MaxHP:{oldStats.maxHealth}{0}\nATK:{oldStats.attack}{1}\nDEF:{oldStats.defense}{2}\nAGI:{oldStats.agility}{3}\n", hpTxt, atkTxt, defTxt, agiTxt);
        
        _costText.SetText(builder);
    }

    public void ChooseThisSkill()
    {
        ////LevelManager.Instance.GetRewardedUnit().ReplaceSkill(_associatedSkills.Item2, _associatedSkills.Item1);
        ///LevelManager.Instance.ClaimReward();
    }
}
