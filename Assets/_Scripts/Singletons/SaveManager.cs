using UnityEngine;

[System.Serializable]
public class PartyData
{
    
}
[System.Serializable]
public class LevelData
{
    public float Difficulty;
    public string LevelName;
    //public RewardType
    public LevelData(float diff, string levelName)
    {
        Difficulty = diff;
        LevelName = levelName;
    }
}
[System.Serializable]
public class GameData
{
    public PartyData partyData;
    public LevelData levelData;
}

public class SaveManager : Singleton<SaveManager>
{
    private string _savePath;
    public GameData gameData;
    
    public const string DATA_PATH = "/gameData.json";

    public void Awake()
    {
        _savePath = Application.persistentDataPath + DATA_PATH;
        gameData = new GameData();
    }

    public void SaveGame()
    {
        gameData.partyData = (PartyData) PartyControls.Instance.CaptureState();
        gameData.levelData = (LevelData) LevelManager.Instance.CaptureState();
        
        string json = JsonUtility.ToJson(gameData, true);
        System.IO.File.WriteAllText(_savePath, json);
        
        Debug.Log("Game Saved");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(_savePath))
        {
            string json = System.IO.File.ReadAllText(_savePath);
            gameData = JsonUtility.FromJson<GameData>(json);
            
            PartyControls.Instance.RestoreState(gameData.partyData);
            LevelManager.Instance.RestoreState(gameData.levelData);
            
            Debug.Log("Game Loaded");
        }
    }
    
}
