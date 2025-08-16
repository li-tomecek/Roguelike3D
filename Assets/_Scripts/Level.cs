using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public Transform StartPosition;
    public Transform RewardPosition;

    [Header("Combat Positions")]
    public List<Transform> PlayerCombatPos;
    public List<Transform> EnemyCombatPos;

    [Header("Patrol Nodes")]
    [SerializeField] private List<Transform> _patrolNodes;

    void Start()
    {
        LevelManager.Instance.SetLevel(this);

        //1. Create new enemies based on difficulty value
        CombatManager.Instance.GetEnemyUnits().Clear();         //just in case
        for (int i = 0; i < 3; i++)
        {
            GameObject unit = EnemyInfoReader.Instance.CreateEnemyWithinDifficulty();
            CombatManager.Instance.GetEnemyUnits().Add(unit.GetComponent<Unit>());
            unit.SetActive(false);
        }

        //2. Put players at start position and play walk animation
        foreach (PlayerUnit unit in PartyController.Instance.GetPartyMembers())
        {
            unit.transform.position = StartPosition.position;
            unit.transform.rotation = StartPosition.rotation;
        }

        InputController.Instance.DisableAllInputMaps();
        StartCoroutine(PartyController.Instance.SetPartyDirectionForDuration(Vector2.up, 1));
        InputController.Instance.ActivateMovementMap();

        //3. Setup Enemy Patrol
        foreach (EnemyUnit unit in CombatManager.Instance.GetEnemyUnits())
        {
            unit.transform.position = _patrolNodes[0].position;
        }

        GameObject patroller = CombatManager.Instance.GetEnemyUnits()[0].gameObject;
        patroller.SetActive(true);
        patroller.GetComponent<Patrol>().PatrolNodes = _patrolNodes;
        patroller.GetComponent<Patrol>().SetState(EnemyState.Patrol);

        //4. SetupCombatPositions for CombatManager
        CombatManager.Instance.SetCombatPositions(PlayerCombatPos, EnemyCombatPos);

        //5. Save Game
        SaveManager.Instance.SaveGame();
    }
}
