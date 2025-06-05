using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
        public static CombatManager Instance;

        [Header("Combat Positions")]
        [SerializeField] private List<Transform> _playerCombatPositions;
        [SerializeField] private List<Transform> _enemyCombatPositions;

        [Header("Combat Units")]
        [SerializeField] private List<Unit> _playerUnits;
        [SerializeField] private List<Unit> _enemyUnits;
        [SerializeField] private float _travelSpeed = 4f;
        
        [Header("Other Parameters")]
        [SerializeField] private GameObject _selectionArrow;
        [SerializeField] private CombatMenu _combatMenu;

        private List<Unit> _combatSequence = new List<Unit>();

        private int _turnIndex;
        private bool _inCombat = false;
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

        // --- Turn Management ---
        // -----------------------
        public void BeginBattle()
        {
            _inCombat = true;

            _selectionArrow = Instantiate(_selectionArrow);
            _selectionArrow.SetActive(false);
            InputController.Instance.ActivateMenuMap();

            _combatSequence.Clear();
            _combatSequence.AddRange(_playerUnits);
            _combatSequence.AddRange(_enemyUnits);

            _combatSequence.OrderByDescending(x => x.GetStats().agility).ToList();           //ToDo: This does not work, and will have to be fixed 

            _turnIndex = -1;

            StartCoroutine(SendUnitsToPosition());      //ToDo: make this into a coroutine so the units actually "walk" there
        }
        private IEnumerator SendUnitsToPosition()
        {
                bool finished = false;
                while (!finished)
                {
                        finished = true;
                        for (int i = 0;(i < _playerUnits.Count && i < _playerCombatPositions.Count); i++)
                        { 
                                if (_playerUnits[i].gameObject.transform.position != _playerCombatPositions[i].position)
                                {
                                        _playerUnits[i].gameObject.transform.position = Vector3.MoveTowards(_playerUnits[i].transform.position,_playerCombatPositions[i].position,_travelSpeed * Time.deltaTime);
                                        _playerUnits[i].transform.LookAt(_playerCombatPositions[i].position);
                                        finished = false;
                                } else
                                        _playerUnits[i].transform.LookAt(_enemyCombatPositions[1].position);    //may want to change the look target
                        }
                
                        for (int i = 0;(i < _enemyUnits.Count && i < _enemyCombatPositions.Count); i++)
                        {
                                if (_enemyUnits[i].gameObject.transform.position != _enemyCombatPositions[i].position)
                                {
                                        _enemyUnits[i].gameObject.transform.position = Vector3.MoveTowards(_enemyUnits[i].transform.position,_enemyCombatPositions[i].position,_travelSpeed * Time.deltaTime);
                                        _enemyUnits[i].transform.LookAt(_enemyCombatPositions[i].position);
                                        finished = false;
                                } else
                                        _enemyUnits[i].transform.LookAt(_playerCombatPositions[1].position);    //may want to change the look target
                        } 
                        
                        yield return 0;
                }   
                NextTurn();
        }
        public void NextTurn()
        {
                if (_playerUnits.Count <= 0 || _enemyUnits.Count <= 0)
                {
                    Debug.Log("Combat Finished.");
                    CameraController.Instance.ToggleCombatCamera();
                    _inCombat = false;
                    InputController.Instance.ActivateMovementMap();

                    return;
                }
                _turnIndex = (_turnIndex == _combatSequence.Count-1) ? 0 : _turnIndex + 1;
                    
            Debug.Log($"--- {_combatSequence[_turnIndex].name}'s Turn --- ");
            _combatSequence[_turnIndex].GetTurnManager().StartTurn();
        }
        public void RemoveFromCombat(Unit unit)
        {
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
        
        
    // --- Targeting Arrow --- 
    // -----------------------
    public void SetTargetArrowPosition(Vector3 position)
    {
        _selectionArrow.SetActive(true);
        _selectionArrow.gameObject.GetComponent<TargetSelectArrow>().SetTarget(position);
    }
    public void SetTargetArrowPositionAtEnemy(int index)
    {
        SetTargetArrowPosition(_enemyUnits[index].gameObject.transform.position);
    } 
    public void SetTargetArrowPositionAtPlayer(int index)
    {
        SetTargetArrowPosition(_playerUnits[index].gameObject.transform.position);
    }
    public void HideArrow()
    {
        _selectionArrow.SetActive(false);
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
    }
    public Unit GetRandomEnemyUnit()
    {
            return _enemyUnits[Random.Range( 0, _enemyUnits.Count)];
    }

    public CombatMenu GetCombatMenu() { return _combatMenu; }

}
