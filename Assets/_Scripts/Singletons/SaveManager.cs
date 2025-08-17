using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerUnitData
{
    public Stats Stats;
    public int CurrentHealth;
    public List<Skill> Skills;
    public GameStats GameStats;   

    public PlayerUnitData(Stats stats, int health, List<Skill> skills, GameStats gameStats)
    {
        Stats = stats;
        CurrentHealth = health;
        Skills = skills;
        GameStats = gameStats;
    }

}
// --------------------------------------------------------------------------

[System.Serializable]
public class PartyData
{
    public List<PlayerUnitData> PartyUnits = new List<PlayerUnitData>();

}
// --------------------------------------------------------------------------

[System.Serializable]
public class LevelData
{
    public float Difficulty;
    public int LevelBuildIndex;
    public PlayerUnit RewardedUnit;

    public List<int> enemyDataIndices;
    public LevelData(float diff, int levelIndex, PlayerUnit rewardedUnit, List<int> enemyIndices)
    {
        Difficulty = diff;
        LevelBuildIndex = levelIndex;
        RewardedUnit = rewardedUnit;
        enemyDataIndices = enemyIndices;
    }
}
// --------------------------------------------------------------------------

[System.Serializable]
public class GameData
{
    public PartyData PartyData;
    public LevelData LevelData;
}

// --------------------------------------------------------------------------
// --------------------------------------------------------------------------

public class SaveManager : Singleton<SaveManager>
{
    private string _savePath;
    public GameData GameData { get; private set; }
    
    public const string DATA_PATH = "/gameData.json";

    public override void Awake()
    {
        base.Awake();
        _savePath = Application.persistentDataPath + DATA_PATH;
        GameData = new GameData();
    }
    public void SaveGame()
    {
        GameData.PartyData = (PartyData) PartyController.Instance.CaptureState();
        GameData.LevelData = (LevelData) LevelManager.Instance.CaptureState();
        
        string json = JsonUtility.ToJson(GameData, true);
        System.IO.File.WriteAllText(_savePath, json);
        
        Debug.Log("Game Saved");
    }

    public bool CanFindSaveData()
    {
        return System.IO.File.Exists(_savePath);
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(_savePath))
        {
            string json = System.IO.File.ReadAllText(_savePath);
            GameData = JsonUtility.FromJson<GameData>(json);
            
            PartyController.Instance.RestoreState(GameData.PartyData);
            LevelManager.Instance.RestoreState(GameData.LevelData);

            Debug.Log("Game Loaded from file");
        }
    }
    
}
