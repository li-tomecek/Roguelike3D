using System;
using TMPro;
using UnityEngine;

public class RewardOption : MonoBehaviour
{
    private Tuple<Skill, Skill> _associatedSkills;

    [SerializeField] TextMeshProUGUI _oldNameText;
    [SerializeField] TextMeshProUGUI _newNameText;
    [SerializeField] TextMeshProUGUI _costText;
    [SerializeField] TextMeshProUGUI _descriptionText;

    public void SetupPanelForSkill(Tuple<Skill, Skill> skills)
    {
        _associatedSkills = skills;

        _oldNameText.SetText(_associatedSkills.Item2.name);
        _newNameText.SetText(_associatedSkills.Item1.name);
        _costText.SetText("BP: " + _associatedSkills.Item1.GetCost());
        _descriptionText.SetText(_associatedSkills.Item1.description);
    }

    public void ChooseThisSkill()
    {
        LevelManager.Instance.GetRewardedUnit().ReplaceSkill(_associatedSkills.Item2, _associatedSkills.Item1);
        LevelManager.Instance.ClaimReward();
    }
}
