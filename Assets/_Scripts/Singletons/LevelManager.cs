using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>, ISaveable
{
    [Header("Level Information")]
    public string CurrentLevelName; 
    public float DifficultyValue;
    
    [Header("Level Rewards")]
    [SerializeField] private PlayerUnit _rewardedUnit;
    [SerializeField] private List<PlayerUnit> _nextRoomRewards;
    [SerializeField] GameObject _combatReward;
    
    private const string PERSISTENT_SCENE_NAME = "PersistentScene";

    // --- Level Load/Unloading ---
    // ----------------------------
    #region
    public void Start()
    {
        SceneManager.sceneLoaded += SetActiveScene;
        //LoadLevel("MainMenu");
    }
    
    private void SetActiveScene(Scene scene, LoadSceneMode mode)
    {
        SceneManager.SetActiveScene(scene);    //so the persistent scene is not the one being unloaded
    }
    
    public void LoadLevel(string levelName)
    {
        if(SceneManager.GetActiveScene() != SceneManager.GetSceneByName(PERSISTENT_SCENE_NAME))     //do not unload the persistent scene
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }
    #endregion
    
    // --- Level Rewards ---
    // ---------------------
    #region
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
    #endregion
    
    // --- State Saving and Restoring ---
    // --------------------
    #region
    public object CaptureState()
    {
        LevelData levelData = new LevelData(DifficultyValue, CurrentLevelName);
        return levelData;
    }

    public void RestoreState(object data)
    {
        try
        {
            LevelData levelData = (LevelData)data;
            CurrentLevelName = levelData.LevelName;
            DifficultyValue = levelData.Difficulty;
            
            LoadLevel(CurrentLevelName);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    #endregion
}
