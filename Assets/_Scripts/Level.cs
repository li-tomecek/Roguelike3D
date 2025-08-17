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

    private float PERCENT_RESTORED_HEALTH = 0.2f;   //20% restored health on entry

    void Start()
    {
        LevelManager.Instance.SetLevel(this);

        //1. Create new enemies based on difficulty value, or based off of loaded save data
        CreateLevelEnemies();

        //2. Player Setup
        PartySetup();

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

    private void CreateLevelEnemies()
    {
        CombatManager.Instance.GetEnemyUnits().Clear();

        if (!EnemyInfoReader.Instance.ShouldUseSaveData())
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
    }

    private void PartySetup()
    {
        // Put in start position, play walk animation, heal, and reset BP
        foreach (PlayerUnit unit in PartyController.Instance.GetPartyMembers())
        {
            unit.transform.position = StartPosition.position;
            unit.transform.rotation = StartPosition.rotation;

            unit.SetBP(0);

            if (unit.GetHealth() < unit.GetStats().maxHealth)
                unit.TakeDamage((int)(PERCENT_RESTORED_HEALTH * (float)unit.GetStats().maxHealth) * -1);

        }

        InputController.Instance.DisableActiveMap();
        StartCoroutine(PartyController.Instance.SetPartyDirectionForDuration(Vector2.up, 0.5f));
        InputController.Instance.EnableActiveMap();
    }
}
