using UnityEngine;

/*
 *  Manages all things related to the interface the player sees during combat.
 *  - Player's Turn Menu
 *  - Damage and Healing Indicators
 *  - Target Selection indicator
 *  - ToDo: Turn Indicator
 */

public class CombatInterface : MonoBehaviour
{
    public static CombatInterface Instance;

    [SerializeField] private PlayerTurnMenu _combatMenu;
    [SerializeField] private GameObject _selectionArrow;
    [SerializeField] DamageIndicator _damageIndicator;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        _selectionArrow = Instantiate(_selectionArrow);
        _selectionArrow.SetActive(false);
    }

    // --- Targeting Arrow --- 
    // -----------------------
    public void SetTargetArrowPosition(Vector3 position)
    {
        _selectionArrow.SetActive(true);
        _selectionArrow.gameObject.GetComponent<TargetSelectArrow>().SetTarget(position);
    }
    public void SetTargetArrowPositionAtEnemy(int index)
    {
        SetTargetArrowPosition(CombatManager.Instance.GetEnemyUnits()[index].gameObject.transform.position);
    }
    public void SetTargetArrowPositionAtPlayer(int index)
    {
        SetTargetArrowPosition(CombatManager.Instance.GetPlayerUnits()[index].gameObject.transform.position);
    }
    public void HideArrow()
    {
        _selectionArrow.SetActive(false);
    }

    // --- Damage Indicator --- 
    // ------------------------
    public void SetDamageIndicator(int damage, Transform target)
    {
        _damageIndicator.gameObject.SetActive(true);
        _damageIndicator.ShowDamageAtTarget(damage, target);
    }

    // --- Getters / Setters ---
    // -------------------------
    public PlayerTurnMenu GetTurnMenu() { return _combatMenu; }

}
