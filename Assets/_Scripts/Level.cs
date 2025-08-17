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

        //1. Create new enemies based on difficulty value, or based off of loaded save data
        CombatManager.Instance.GetEnemyUnits().Clear();

        if(!EnemyInfoReader.Instance.ShouldUseSaveData())
            EnemyInfoReader.Instance.chosenRowIndices.Clear();

        for (int i = 0; i < 3; i++)
        {
            GameObject unit;
            if (EnemyInfoReader.Instance.ShouldUseSaveData())
                unit = EnemyInfoReader.Instance.CreateEnemyDataFromRow(EnemyInfoReader.Instance.chosenRowIndices[i]);
            else
                unit = EnemyInfoReader.Instance.CreateEnemyWithinDifficulty();
                
            CombatManager.Instance.GetEnemyUnits().Add(unit.GetComponent<Unit>());
            unit.SetActive(false);
        }
        EnemyInfoReader.Instance.SetShouldUseSaveData(false);

        //2. Put players at start position and play walk animation
        foreach (PlayerUnit unit in PartyController.Instance.GetPartyMembers())
        {
            unit.transform.position = StartPosition.position;
            unit.transform.rotation = StartPosition.rotation;
        }

        InputController.Instance.DisableActiveMap();
        StartCoroutine(PartyController.Instance.SetPartyDirectionForDuration(Vector2.up, 0.5f));
        InputController.Instance.EnableActiveMap();

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
