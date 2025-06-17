using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerTurnMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _unitNameText;
    [SerializeField] private TextMeshProUGUI _unitBPText;

    [Header("Action Menu")]
    private GameObject _actionPanel;
    [SerializeField] private Button _defaultSelectedButton;
    
    [Header("Skills Menu")]
    private GameObject _skillsPanel;
    [SerializeField] private Button _skill1Btn;
    [SerializeField] private Button _skill2Btn;

    private PlayerUnit _unit;
    private void Start()
    {
        _actionPanel = GameObject.Find("ActionPanel");
        _skillsPanel = GameObject.Find("SkillsPanel");
        
        _skillsPanel.SetActive(false);
        gameObject.SetActive(false);
    }

    public void SetupMenu(PlayerUnit unit)
    {
        gameObject.SetActive(true);
        _actionPanel.SetActive(true);   
        
        _unit = unit;
        _unitNameText.SetText(unit.name);
        _unitBPText.SetText($"BP: {_unit.GetBP()}");

        if (unit.GetSkills()[0] != null)
        {
            _skill1Btn.GetComponentInChildren<TextMeshProUGUI>().SetText($"{ unit.GetSkills()[0].name}\n[{unit.GetSkills()[0].GetCost()} BP]");
            if (unit.GetSkills()[0].GetCost() > unit.GetBP())
                _skill1Btn.interactable = false;
            else
                _skill1Btn.interactable = true;
        }
        if (unit.GetSkills()[1] != null)
        {
            _skill2Btn.GetComponentInChildren<TextMeshProUGUI>().SetText($"{unit.GetSkills()[1].name}\n[{unit.GetSkills()[1].GetCost()} BP]");
            if (unit.GetSkills()[1].GetCost() > unit.GetBP())
                _skill2Btn.interactable = false;
            else
                _skill2Btn.interactable = true;

        }

        InputController.Instance.CancelEvent.AddListener(BackToActionMenu);
    }

    private void CloseMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        _skillsPanel.SetActive(false);

        InputController.Instance.CancelEvent.RemoveAllListeners();

        gameObject.SetActive(false);

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

    
    
    
    // --- Input Reading Methods ---
    //------------------------------
    public void BackToActionMenu()
    {
        //going from skills panel back to action panel
        if (_skillsPanel.activeSelf)
        {
            _actionPanel.SetActive(true);
            _skillsPanel.SetActive(false);
        }
    }



}
