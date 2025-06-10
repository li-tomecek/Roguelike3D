using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CombatMenu : MonoBehaviour
{
    [Header("Action Menu")]
    private GameObject _actionPanel;
    
    [Header("Skills Menu")]
    private GameObject _skillsPanel;
    [SerializeField] private TextMeshProUGUI _unitNameText;
    [SerializeField] private TextMeshProUGUI _skill1Text;
    [SerializeField] private TextMeshProUGUI _skill2Text;

    private PlayerUnit _unit;
    private void Start()
    {
        _actionPanel = GameObject.Find("ActionPanel");
        _skillsPanel = GameObject.Find("SkillsPanel");
        
        _actionPanel.SetActive(false);
        _skillsPanel.SetActive(false);
    }

    public void SetupCombatMenu(PlayerUnit unit)
    {
        _unit = unit;
        _unitNameText.SetText(unit.name);

        if (unit.GetSkills()[0] != null)
            _skill1Text.SetText(unit.GetSkills()[0].name);
        if (unit.GetSkills()[1] != null)
            _skill2Text.SetText(unit.GetSkills()[1].name);
        
        _actionPanel.SetActive(true);
    }

    private void CloseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _actionPanel.SetActive(false);
        _skillsPanel.SetActive(false);
    }

    
    // --- Button On Click Methods ---
    // -------------------------------
    public void AttackButtonPressed()
    {
        //select target for default attack
        _unit.GetPlayerTurnManager().ChooseTargetForSkill(_unit.GetDefaultSkill());
        CloseMenu();
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
        _unit.GetPlayerTurnManager().ChooseTargetForSkill(_unit.GetSkills()[skillIndex]);   //ToDo: exception handling here! Or just a regular check
        CloseMenu();
    }

}
