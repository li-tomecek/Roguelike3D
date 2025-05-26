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
        //----------------------------------------------------
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
                SendUnitsToPosition();
                
                _combatSequence.Clear();
                _combatSequence.AddRange(_playerUnits);
                _combatSequence.AddRange(_enemyUnits);

                _combatSequence.OrderByDescending(x => x.GetStats().agility);           //.ToList();

                _turnIndex = -1;
                NextTurn();
        }

        public void NextTurn()
        {
                _turnIndex++;
                _combatSequence[_turnIndex].GetTurnManager().StartTurn();
        }
        
        private void SendUnitsToPosition()
        {
                //TODO: change to some kind of translation so it looks like the units are walking into position
                
                for (int i = 0;(i < _playerUnits.Count && i < _playerCombatPositions.Count); i++)
                {
                        _playerUnits[i].gameObject.transform.position = _playerCombatPositions[i].position;
                }
                
                for (int i = 0;(i < _enemyUnits.Count && i < _enemyCombatPositions.Count); i++)
                {
                        _enemyUnits[i].gameObject.transform.position = _enemyCombatPositions[i].position;
                }
        }
        
}
