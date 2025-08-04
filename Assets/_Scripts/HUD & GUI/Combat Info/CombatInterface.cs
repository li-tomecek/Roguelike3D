using UnityEngine;

/*
 *  Manages all things related to the interface the player sees during combat.
 *  - Player's Turn Menu
 *  - Damage and Healing Indicators
 *  - Target Selection indicator
 *  - ToDo: Turn Indicator
 */

public class CombatInterface : Singleton<CombatInterface>
{
    [SerializeField] private TargetSelectArrow _selectionArrowPrefab;
    [SerializeField] private PlayerTurnMenu _turnMenu;
    [SerializeField] private GameObject _damageIndicatorPrefab;
    [SerializeField] private int _pooledIndicatorAmount;
    [SerializeField] private GameObject _gameOverPrefab;


    private ObjectPool _damageIndicators;

    public override void Awake()
    {
        base.Awake();
        Setup();
        CombatManager.Instance.OnGameOver.AddListener(OpenGameOverScreen);
    }
    public void Setup()
    {
        _damageIndicators = new ObjectPool(_damageIndicatorPrefab, _pooledIndicatorAmount);
        
        _selectionArrowPrefab = Instantiate(_selectionArrowPrefab);
        _selectionArrowPrefab.gameObject.SetActive(false);

        _gameOverPrefab = Instantiate(_gameOverPrefab);
        _gameOverPrefab.gameObject.SetActive(false);

        _turnMenu = Instantiate(_turnMenu).GetComponent<PlayerTurnMenu>();
    }

    // --- Targeting Arrow --- 
    // -----------------------
    public void SetTargetArrowPosition(Vector3 position)
    {
        _selectionArrowPrefab.gameObject.SetActive(true);
        _selectionArrowPrefab.SetTarget(position);
    }
    public void HideArrow()
    {
        _selectionArrowPrefab.gameObject.SetActive(false);
    } 

    // --- Damage Indicator --- 
    // ------------------------
    public void SetIndicator(string str, Transform target, UnityEngine.Color color)
    {
        _damageIndicators.GetActivePooledObject().GetComponent<DamageIndicator>().ShowIndicatorAtTarget(str, target, color);
    }
    public void SetIndicator(string str, Transform target)
    {

        _damageIndicators.GetActivePooledObject().GetComponent<DamageIndicator>().ShowIndicatorAtTarget(str, target, UnityEngine.Color.black);
    }

    // --- Game Over Screen --- 
    // ------------------------
    public void OpenGameOverScreen()
    {
        _gameOverPrefab.SetActive(true);
    }

    // --- Getters / Setters ---
    // -------------------------
    public PlayerTurnMenu GetTurnMenu() { return _turnMenu; }

}
