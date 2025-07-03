using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private PlayerUnit _rewardedUnit;
    [SerializeField] private List<PlayerUnit> _nextRoomRewards;

    [SerializeField] GameObject _combatReward;

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

    // --- Level Rewards ---
    // ---------------------
    public PlayerUnit GetRewardedUnit() { return _rewardedUnit; }

    public void SpawnReward()
    {
        _combatReward = Instantiate(_combatReward, gameObject.transform);
    }

    public void ClaimReward()
    {
        _combatReward.GetComponentInChildren<CombatReward>().CloseMenu();
        Destroy(_combatReward);
    }
}
