using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class CombatManager : MonoBehaviour
{
        public static CombatManager Instance;
        
        [SerializeField] private List<Transform> _playerCombatPositions;
        [SerializeField] private List<Transform> _enemyCombatPositions;
        
        [SerializeField] private List<Unit> _playerUnits;
        [SerializeField] private List<Unit> _enemyUnits;
        
        //LinkedList for combat sequence?
        private List<Unit> _combatSequence = new List<Unit>();
        private int _turnIndex;

        public bool _inCombat = false;
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
    
        // --- Combat Methods ---
        public void BeginBattle()
        {
            _inCombat = true;
            InputController.Instance.ActivateMenuMap();

            SendUnitsToPosition();      //ToDo: make this into a coroutine so the units actually "walk" there
                
                _combatSequence.Clear();
                _combatSequence.AddRange(_playerUnits);
                _combatSequence.AddRange(_enemyUnits);

                _combatSequence.OrderByDescending(x => x.GetStats().agility).ToList();           //.ToList();

                _turnIndex = -1;
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
        
        private void SendUnitsToPosition()
        {
                //TODO: change to some kind of translation so it looks like the units are walking into position
                
                for (int i = 0;(i < _playerUnits.Count && i < _playerCombatPositions.Count); i++)
                {
                    _playerUnits[i].gameObject.transform.position =  _playerCombatPositions[i].position;
                }
                
                for (int i = 0;(i < _enemyUnits.Count && i < _enemyCombatPositions.Count); i++)
                {
                     _enemyUnits[i].gameObject.transform.position = _enemyCombatPositions[i].position;
                }
        }

        public Unit GetRandomPlayerUnit()
        {
                return _playerUnits[Random.Range( 0, _playerUnits.Count)];
        }

        public Unit GetRandomEnemyUnit()
        {
                return _enemyUnits[Random.Range( 0, _enemyUnits.Count)];
        }

        public void RemoveFromCombat(Unit unit)
        {
                if(_playerUnits.Contains(unit))
                        _playerUnits.Remove(unit);
                else if (_enemyUnits.Contains(unit))
                        _enemyUnits.Remove(unit);
        }
}
