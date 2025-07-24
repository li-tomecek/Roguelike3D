using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>, ISaveable
{
    [Header("Level Rewards")]
    [SerializeField] private PlayerUnit _rewardedUnit;
    [SerializeField] private List<PlayerUnit> _nextRoomRewards;
    [SerializeField] GameObject _combatReward;

    // --- Level Load/Unloading ---
    // ----------------------------
    public void Start()
    {
        SceneManager.sceneLoaded += SetActiveScene;
    }
    
    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);    //so the persistent scene is not the one being unloaded
    }
    
    public void LoadLevel(string levelName)
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }
    
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
