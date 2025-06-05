using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatMenu : MonoBehaviour
{
    [Header("Action Menu")]
    private GameObject _actionPanel;
    
    [Header("Skills Menu")]
    private GameObject _skillsPanel;
    [SerializeField] private Button _skill1Btn;
    [SerializeField] private Button _skill2Btn;

    private Unit _unit;
    private void Start()
    {
        _actionPanel = GameObject.Find("ActionPanel");
        _skillsPanel = GameObject.Find("SkillsPanel");
        
        _actionPanel.SetActive(false);
        _skillsPanel.SetActive(false);
    }

    public void SetupCombatMenu(Unit unit)
    {
        _unit = unit;

        /*if (unit.GetSkills()[0] != null)
            _skill1Btn.GetComponent<TextMeshProUGUI>().text = unit.GetSkills()[0].name;
        if (unit.GetSkills()[1] != null)
            _skill2Btn.GetComponent<TextMeshProUGUI>().text = unit.GetSkills()[1].name;*/
        
        _actionPanel.SetActive(true);
    }

    public void AttackButtonPressed()
    {
        //select target for default attack
    }
    
    public void SkillsButtonPressed()
    {
        //open skills menu, deactivate action menu
        _actionPanel.SetActive(false);
        _skillsPanel.SetActive(true);    
    }

    public void GuardButtonPressed()
    {
        //Future implementation: unit takes half damage on next attack against them?
    }

    public void SkillButtonPressed(int skillIndex)
    {
        //select target for selected skill
    }

}
