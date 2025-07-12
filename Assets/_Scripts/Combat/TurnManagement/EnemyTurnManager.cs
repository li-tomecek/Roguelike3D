
public class EnemyTurnManager : TurnManager
{

    public void Start()
    {
        this.unit = GetComponent<EnemyUnit>();
    }

    // --- Overriden methods ---
    // -------------------------
    public override void StartTurn()
    {
        base.StartTurn();

        //this must be the last statement in the function    
        StartCoroutine(PlayTurnSequence(unit.GetDefaultSkill(), CombatManager.Instance.GetRandomPlayerUnit()));
        
    }
}
