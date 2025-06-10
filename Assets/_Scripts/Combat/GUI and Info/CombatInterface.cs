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

    [SerializeField] private TargetSelectArrow _selectionArrow;
    [SerializeField] private PlayerTurnMenu _turnMenu;
    [SerializeField] DamageIndicator _damageIndicatorPrefab;
    


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
        _selectionArrow.gameObject.SetActive(false);

        _turnMenu = Instantiate(_turnMenu).GetComponent<PlayerTurnMenu>();

        _damageIndicatorPrefab = Instantiate(_damageIndicatorPrefab).GetComponent<DamageIndicator>();
        _damageIndicatorPrefab.gameObject.SetActive(false);

    }

    // --- Targeting Arrow --- 
    // -----------------------
    public void SetTargetArrowPosition(Vector3 position)
    {
        _selectionArrow.gameObject.SetActive(true);
        _selectionArrow.SetTarget(position);
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
        _selectionArrow.gameObject.SetActive(false);
    }

    // --- Damage Indicator --- 
    // ------------------------
    public void SetDamageIndicator(int damage, Transform target)
    {
        _damageIndicatorPrefab.gameObject.SetActive(true);
        _damageIndicatorPrefab.ShowDamageAtTarget(damage, target);
    }

    // --- Getters / Setters ---
    // -------------------------
    public PlayerTurnMenu GetTurnMenu() { return _turnMenu; }

}
