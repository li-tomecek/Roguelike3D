using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/*
 *  Handles combat setup and turn sequence and management.
 *  Handles global combat logic aside from anything graphical.
 */
public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    [Header("Combat Units")]
    private List<Unit> _playerUnits;
    [SerializeField] private List<Unit> _enemyUnits;
    [SerializeField] private float _percentDamageOnDisadvantage = 0.1f;

    [Header("Combat Positions")]
    [SerializeField] private List<Transform> _playerCombatPositions;
    [SerializeField] private List<Transform> _enemyCombatPositions;
    [SerializeField] private float _travelSpeed = 2f;
    [SerializeField] private float _targetDistanceThreshold = 0.1f;


    private GameObject[] _obstacles;

    private List<Unit> _combatSequence = new List<Unit>();

    private int _turnIndex;
    private bool _inCombat = false;
    private bool _playerAdvantage;

    //---------------------------------------------------
    //---------------------------------------------------
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
    }

    // --- Setup  ---
    // --------------
    public void BeginBattle(bool playerAdvantage)
    {
        _playerUnits = PartyControls.Instance.GetPartyMembers().Cast<Unit>().ToList(); //Get player units from party manager

        _playerAdvantage = playerAdvantage;
        _obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
       
        CameraController.Instance.ToggleCombatCamera();
        InputController.Instance.ActivateMenuMap();
        _inCombat = true;
        _turnIndex = -1;


        //disable NavAgetn for all patrolling enemies
        foreach (Unit enemy in _enemyUnits)
        {
            if(enemy.TryGetComponent<Patrol>(out Patrol patroller))
            {
                patroller.PatrolToCombat();
            }
        }

        //Add and units to combatSequence List and order them by agility
        _combatSequence.Clear();
        _combatSequence.AddRange(_playerUnits);
        _combatSequence.AddRange(_enemyUnits);

        _combatSequence = _combatSequence.OrderByDescending(x => x.GetStats().agility).ToList(); 

        //Start required coroutines
        StartCoroutine(CombatSetupSequence());  
    }
    private IEnumerator CombatSetupSequence()
    { 
        //activate hidden enemies
        foreach(Unit unit in _enemyUnits)
        {
            unit.gameObject.SetActive(true);
        }

        //Deactivate obstacles that will block vision and position setup
        foreach (GameObject go in _obstacles)
        {
            go.SetActive(false);
        }

        //move to start positions
        yield return SendUnitsToPosition();
        
        //activate health bars
        foreach (Unit unit in _combatSequence)
        {
            unit.GetHealthBar().gameObject.SetActive(true);
        }
        
        //Apply disadvantage damage
        if (_playerAdvantage)    // all enemies start with damage equal to 10% of max health
            ApplyDisadvantageDamage(_enemyUnits);
        else                    // all players start with damage equal to 10% of max health
            ApplyDisadvantageDamage(_playerUnits);

        //Start the first turn
        NextTurn();

    }
    private IEnumerator SendUnitsToPosition()
    {

        List<Coroutine> routines = new List<Coroutine>();

        for (int i = 0; i < _playerUnits.Count; i++)
        {
            _playerUnits[i].GetComponent<PlayerAnimator>().SetMovementSpeed(_travelSpeed);
            _playerUnits[i].GetComponent<PlayerAnimator>().SetCombatAnimations(true);

            routines.Add(StartCoroutine(_playerUnits[i].MoveTo(_playerCombatPositions[i].position, _travelSpeed, _targetDistanceThreshold)));
        }

        for (int i = 0; i < _enemyUnits.Count; i++)
        {
            routines.Add(StartCoroutine(_enemyUnits[i].MoveTo(_enemyCombatPositions[i].position, _travelSpeed, _targetDistanceThreshold)));
        }

        for (int i = 0; i < routines.Count; i++)
        {
            yield return routines[i];
        }
    }
    private void ApplyDisadvantageDamage(List<Unit> unitList)
    {
        int damage;
        foreach (Unit unit in unitList)
        {
            damage = Math.Max((int)(unit.GetStats().maxHealth * _percentDamageOnDisadvantage), 1); //Unit must take at least 1 damage(if possible)
            damage = Math.Min(damage, (unit.GetHealth() - 1));                                      //unit must remain at at least 1HP

            unit.TakeDamage(damage);
            CombatInterface.Instance.SetIndicator(damage.ToString(), unit.gameObject.transform);

        }
    }

    // --- Turn Management -----
    // -------------------------
    public void NextTurn()
    {
            if (_playerUnits.Count <= 0 || _enemyUnits.Count <= 0)
            {
                EndEncounter();
                return;
            }
            _turnIndex = (_turnIndex == _combatSequence.Count-1) ? 0 : _turnIndex + 1;
                    
        Debug.Log($"--- {_combatSequence[_turnIndex].name}'s Turn --- ");
        _combatSequence[_turnIndex].GetTurnManager().StartTurn();
    }
    private void EndEncounter()
    {
        Debug.Log("Combat Finished.");

        CameraController.Instance.ToggleCombatCamera();

        //re-enable the map obstacles
        foreach (GameObject go in _obstacles)
        {
            go.SetActive(true);
        }

        
        if(_playerUnits.Count > 0)  //PLAYER WON
        {
            _inCombat = false;
            InputController.Instance.ActivateMovementMap();
            
            foreach(PlayerUnit unit in PartyControls.Instance.GetPartyMembers())
            {
                unit.gameObject.SetActive(true);
                unit.GetComponent<PlayerAnimator>().EndCombatAnimations();
                
                if (unit.GetHealth() <= 0)
                    unit.SetHealth(1);                              //revive "dead" units to 1HP
                
                unit.GetHealthBar().gameObject.SetActive(false);    //hide all health bars
            }

            LevelManager.Instance.SpawnReward();
        }
        else   //GAME OVER
        {
            CombatInterface.Instance.OpenGameOverScreen();
        }

    }
    public void RemoveFromCombat(Unit unit)
    {    
        if (unit.gameObject.GetComponent<PlayerAnimator>())
        {
            unit.GetComponent<PlayerAnimator>().SetDeathAnimations(true);
            unit.GetHealthBar().gameObject.SetActive(false);
        }    
        else
            unit.gameObject.SetActive(false);


        if (_playerUnits.Contains(unit))
        {
            _playerUnits.Remove(unit);
        }
        else if (_enemyUnits.Contains(unit))
        {
            _enemyUnits.Remove(unit);
        }

        if (_combatSequence.IndexOf(unit) < _turnIndex)
                _turnIndex--;
             
        _combatSequence.Remove(unit);
    }

    // --- Getters / Setters ---
    // -------------------------
    public List<Unit> GetEnemyUnits() { return _enemyUnits; }
    public List<Unit> GetPlayerUnits() { return _playerUnits; }
    public List<Unit> GetCombatSequence() { return _combatSequence; } 
    public bool InCombat() { return _inCombat; }
    public Unit GetRandomPlayerUnit()
    {
            return _playerUnits[Random.Range(0, _playerUnits.Count)];
    }   //temp







    private IEnumerator OldSendUnitsToPosition()
    {
        //for the animator
        foreach (var unit in _combatSequence)
        {
            if (unit.gameObject.GetComponent<PlayerAnimator>())
            {
                unit.GetComponent<PlayerAnimator>().SetMovementSpeed(_travelSpeed);
                unit.GetComponent<PlayerAnimator>().SetCombatAnimations(true);
            }
        }

        bool atDestination, finished = false;
        Vector3 direction;

        while (!finished)
        {
            Unit unit;
            finished = true;
            //Move all player units into their start position
            for (int i = 0; (i < _playerUnits.Count && i < _playerCombatPositions.Count); i++)
            {
                unit = _playerUnits[i];

                direction = (_playerCombatPositions[i].transform.position - unit.transform.position);
                direction.y = 0f;   //we don't want to adjust their height.

                atDestination = direction.magnitude < _targetDistanceThreshold;
                if (!atDestination)
                {
                    direction.Normalize();
                    unit.gameObject.GetComponent<CharacterController>().Move(direction * _travelSpeed * Time.deltaTime);
                }
                else
                {
                    direction = _enemyCombatPositions[1].transform.position - unit.transform.position;
                    direction.y = 0f;
                    unit.GetComponent<PlayerAnimator>().SetMovementSpeed(0f);

                }

                unit.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                finished &= atDestination;

            }


            //Move all enemy units into their start position
            for (int i = 0; (i < _enemyUnits.Count && i < _enemyCombatPositions.Count); i++)
            {
                unit = _enemyUnits[i];

                direction = (_enemyCombatPositions[i].transform.position - unit.transform.position);
                direction.y = 0f;   //we don't want to adjust their height

                atDestination = direction.magnitude < _targetDistanceThreshold;
                if (!atDestination)
                {
                    direction.Normalize();
                    unit.gameObject.GetComponent<CharacterController>().Move(direction * _travelSpeed * Time.deltaTime);
                }
                else
                {
                    direction = _playerCombatPositions[1].transform.position - unit.transform.position;
                    direction.y = 0f;
                }

                unit.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

                finished &= atDestination;

            }

            yield return null;
        }
    }
}
