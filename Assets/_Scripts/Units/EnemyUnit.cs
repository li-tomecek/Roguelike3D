
using UnityEngine;

public class EnemyUnit : Unit
{

    [Header("Skill-Scoring Constants")]
    [Range(0.0f, 1.0f)] public float MaxPriorityThreshold = 0.5f;   //all move scores >= (MaxPriorityThreshold * highestScore) are considered for possible moves

    //The sum of these does not necessarily have to be 1
    [Range(0.0f, 1.0f)] public float C_Heal = 1f;
    [Range(0.0f, 1.0f)] public float C_Attack = 1f;
    [Range(0.0f, 1.0f)] public float C_StatMod = 1f;

    protected override void Awake()
    {
        turnManager = gameObject.AddComponent<EnemyTurnManager>();
        base.Awake();
    }
    
}
