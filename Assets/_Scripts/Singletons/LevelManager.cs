using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>, ISaveable
{
    [Header("Level Information")]
    public string CurrentLevelName { get; private set; }
    public float DifficultyValue { get; private set; }
    public PlayerUnit RewardedUnit;
    [SerializeField] private float _difficultyIncrement = 0.2f;  
    public Level CurrentLevel { get; private set; }
    
    [Header("Level Rewards")]
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
        LoadLevel("Level_C");
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

    public void SetLevel(Level level) 
    { 
        this.CurrentLevel = level; 
        this.CurrentLevelName = SceneManager.GetActiveScene().name; 
    }
    #endregion
    
    // --- Level Rewards ---
    // ---------------------
    #region
    public PlayerUnit GetRewardedUnit() { return RewardedUnit; }

    public void PostCombat()
    {
        SpawnReward();
        DifficultyValue += _difficultyIncrement;    //increase the difiiculty score to control enemy difficulty levels.
        DifficultyValue = Math.Min(DifficultyValue, 1);
    }
    public void SpawnReward()
    {
        _combatReward = Instantiate(_combatReward, CurrentLevel.RewardPosition);
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
    // ----------------------------------
    #region
    public object CaptureState()
    {
        LevelData levelData = new LevelData(DifficultyValue, CurrentLevelName, RewardedUnit);
        return levelData;
    }

    public void RestoreState(object data)
    {
        try
        {
            LevelData levelData = (LevelData)data;
            CurrentLevelName = levelData.LevelName;
            DifficultyValue = levelData.Difficulty;
            RewardedUnit = levelData.RewardedUnit;
            
            LoadLevel(CurrentLevelName);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
    #endregion
}
