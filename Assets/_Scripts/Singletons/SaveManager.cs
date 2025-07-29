using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
    public Stats Stats;
    public int CurrentHealth;
    public Skill Skill_1;
    public Skill Skill_2;

    //enemies will also need to save their AI constants. Or maybe, we just save the name for when we want to read them fom a cvs file.
}
// --------------------------------------------------------------------------

[System.Serializable]
public class PartyData
{
    public List<UnitData> PartyUnits = new List<UnitData>();

}
// --------------------------------------------------------------------------

[System.Serializable]
public class LevelData
{
    public float Difficulty;
    public string LevelName;
    public PlayerUnit RewardedUnit;
    public LevelData(float diff, string levelName, PlayerUnit rewardedUnit)
    {
        Difficulty = diff;
        LevelName = levelName;
        RewardedUnit = rewardedUnit;
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

    public void Update()    //TEMPORARY!!!
    {
        if(Input.GetKeyDown(KeyCode.K))
            SaveGame();
        
        if(Input.GetKeyDown(KeyCode.L))
            LoadGame();
    }
    public void SaveGame()
    {
        GameData.PartyData = (PartyData) PartyControls.Instance.CaptureState();
        GameData.LevelData = (LevelData) LevelManager.Instance.CaptureState();
        
        string json = JsonUtility.ToJson(GameData, true);
        System.IO.File.WriteAllText(_savePath, json);
        
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(_savePath))
        {
            string json = System.IO.File.ReadAllText(_savePath);
            GameData = JsonUtility.FromJson<GameData>(json);
            
            //PartyControls.Instance.RestoreState(GameData.PartyData);
            LevelManager.Instance.RestoreState(GameData.LevelData);
            
            //LevelManager.Instance

            Debug.Log("Game Loaded from file");
        }
    }
    
}
