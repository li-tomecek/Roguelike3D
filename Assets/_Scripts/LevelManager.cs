using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private PlayerUnit _rewardedUnit;
    [SerializeField] private List<PlayerUnit> _nextRoomRewards;

    [SerializeField] GameObject _combatReward;

    // --- Level Rewards ---
    // ---------------------
    public PlayerUnit GetRewardedUnit() { return _rewardedUnit; }

    public void SpawnReward()
    {
        _combatReward = Instantiate(_combatReward, gameObject.transform);
        foreach (var door in Resources.FindObjectsOfTypeAll<Door>())
        {
            door.UnlockDoor();
        }
    }

    public void ClaimReward()
    {
        _combatReward.GetComponentInChildren<CombatReward>().CloseMenu();
        Destroy(_combatReward);
    }
}
