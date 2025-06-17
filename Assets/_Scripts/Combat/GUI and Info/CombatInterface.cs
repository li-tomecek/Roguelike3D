using System.Collections.Generic;
using System.Drawing;
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
    [SerializeField] private GameObject _damageIndicatorPrefab;
    [SerializeField] private int _pooledIndicatorAmount;

    private ObjectPool _damageIndicators;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Setup();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Setup()
    {
        _damageIndicators = new ObjectPool(_damageIndicatorPrefab, _pooledIndicatorAmount);
        
        _selectionArrow = Instantiate(_selectionArrow);
        _selectionArrow.gameObject.SetActive(false);

        _turnMenu = Instantiate(_turnMenu).GetComponent<PlayerTurnMenu>();
    }

    // --- Targeting Arrow --- 
    // -----------------------
    public void SetTargetArrowPosition(Vector3 position)
    {
        _selectionArrow.gameObject.SetActive(true);
        _selectionArrow.SetTarget(position);
    }
    public void HideArrow()
    {
        _selectionArrow.gameObject.SetActive(false);
    } 

    // --- Damage Indicator --- 
    // ------------------------
    public void SetDamageIndicator(int damage, Transform target, UnityEngine.Color color)
    {
        _damageIndicators.GetActivePooledObject().GetComponent<DamageIndicator>().ShowDamageAtTarget(damage, target, color);
    }
    public void SetDamageIndicator(int damage, Transform target)
    {

        _damageIndicators.GetActivePooledObject().GetComponent<DamageIndicator>().ShowDamageAtTarget(damage, target, UnityEngine.Color.black);
    }

    // --- Getters / Setters ---
    // -------------------------
    public PlayerTurnMenu GetTurnMenu() { return _turnMenu; }

}
