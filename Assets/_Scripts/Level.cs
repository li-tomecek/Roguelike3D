using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform StartPosition { get; private set; }
    public Transform RewardPosition { get; private set; }

    [Header("Combat Positions")]
    public List<Transform> _playerCombatPos;
    public List<Transform> _enemyCombatPos;

    [Header("Patrol Nodes")]
    [SerializeField] private List<Transform> _patrolNodes;

    void Start()
    {
        LevelManager.Instance.SetLevel(this);
        
        //1. Create new enemies based on difficulty value
        //CombatManager.Instance.GetEnemyUnits().Clear();         //just in case
        
        //2. Put players at start position
        foreach (PlayerUnit unit in PartyControls.Instance.GetPartyMembers())
        {
            unit.transform.position = StartPosition.position;
            unit.transform.rotation = StartPosition.rotation;
        }

        //3. Setup Enemy Patrol
        CombatManager.Instance.GetEnemyUnits()[0].transform.position = _patrolNodes[0].position;
        CombatManager.Instance.GetEnemyUnits()[0].GetComponent<Patrol>().PatrolNodes = _patrolNodes;
        CombatManager.Instance.GetEnemyUnits()[0].GetComponent<Patrol>().SetState(EnemyState.Patrol);

    }

    // Update is called once per frame
    void OnDestroy()
    {
       //do nothing?
    }
}
