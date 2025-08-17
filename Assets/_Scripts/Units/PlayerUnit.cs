
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerUnit : Unit
{
    private GameStats _gameStats;

    public UnityEvent OnTurnStart = new UnityEvent();
    public UnityEvent OnTurnEnd = new UnityEvent();

    protected override void Awake()
    {
        turnManager = gameObject.AddComponent<PlayerTurnManager>();
        base.Awake();
    }

    public void Start()
    {
        CombatManager.Instance.OnCombatWin.AddListener(Resurrect);
        OnDeath.AddListener(IncrementDeathCounter);
    }

    private void Resurrect()
    {
        if (_health <= 0)
            _health = 1;
    }
    public PlayerTurnManager GetPlayerTurnManager()
    {
        return (PlayerTurnManager)turnManager;
    }
    public override IEnumerator MoveTo(Vector3 targetPosition, float travelSpeed, float acceptedRadius)
    {
      
        this.GetComponent<PlayerAnimator>().SetMovementSpeed(travelSpeed);

        yield return base.MoveTo(targetPosition, travelSpeed, acceptedRadius);

        this.GetComponent<PlayerAnimator>().SetMovementSpeed(0f);

    }
    public override IEnumerator RotateTo(Vector3 lookVector, float rotationSpeed)
    {
        this.GetComponent<PlayerAnimator>().SetMovementSpeed(rotationSpeed);

        yield return base.RotateTo(lookVector, rotationSpeed);

        this.GetComponent<PlayerAnimator>().SetMovementSpeed(0f);
    }

    public void IncrementDeathCounter() { _gameStats.deaths++; }
    public void IncrementKillCounter() { _gameStats.kills++; }
    public void IncrementUpgradeCounter() { _gameStats.upgrades++; }

    public GameStats GetGameStats() { return _gameStats; }
    internal void SetGameStats(GameStats gameStats) {_gameStats = gameStats; }
}

public struct GameStats
{
    public int deaths;
    public int kills;
    public int upgrades;
}
