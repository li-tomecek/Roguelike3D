using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyInfoReader : Singleton<EnemyInfoReader>
{
    #region Fields and Properties
    [Header("Enemy Prefabs")] 
    [SerializeField] private GameObject _easyPrefab;
    [SerializeField] private GameObject _mildPrefab;
    [SerializeField] private GameObject _moderatePrefab;
    [SerializeField] private GameObject _hardPrefab;

    [Header("Difficulty Thresholds")]      //ToDo: Either read these values from file, or write to file so they dont both have to be adjusted.
    [SerializeField][Range(0.0f, 1.0f)] private float _mildThreshold = 0.2f;
    [SerializeField][Range(0.0f, 1.0f)] private float _moderateThreshold = 0.6f;
    [SerializeField][Range(0.0f, 1.0f)] private float _hardThreshold = 1f;

    private float _difficultyVariance = 0.2f;
    
    //Constants -- these change only if the format of thet csv file itself changes
    private const string FILE_NAME = "EnemyInfo";
    private const int DIFFICULTY_COLUMN = 0;
    private const int NAME_COLUMN = 1;
    private const int SKILLS_COLUMN = 6;
    private const int AI_COLUMN = 8;
    
    private string[] _rows;
    
    #endregion
    public override void Awake()
    {
        base.Awake();
        
        TextAsset csvFile = Resources.Load<TextAsset>(FILE_NAME);
        
        if (csvFile != null)            //Read the enemy info file
        {
            _rows = csvFile.text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        }
        else
        {
            Debug.LogError($"There is no file \"{FILE_NAME}\" in Resources folder.");
        }

        //the difficulty variance must be at least half of the size of the largesr difference between thresholds. Otherwise we risk getting an infinite loop when reading enemies
        _difficultyVariance = Math.Max(_mildThreshold / 2f, Math.Abs(_moderateThreshold - _mildThreshold)/2f);
        _difficultyVariance = Math.Max(_difficultyVariance, Math.Abs(_hardThreshold - _moderateThreshold)/2f);



    }

    public GameObject CreateEnemyWithinDifficulty()
    {
        float rowDifficulty;
        int rowIndex;
        string[] cols;

        do
        {
            rowIndex = UnityEngine.Random.Range(1, _rows.Length);       // Row 0 is the column names
            cols = _rows[rowIndex].Split(',');
            rowDifficulty = float.Parse(cols[DIFFICULTY_COLUMN]);
        } 
        while (rowDifficulty > (LevelManager.Instance.DifficultyValue + _difficultyVariance) 
            || rowDifficulty < (LevelManager.Instance.DifficultyValue - _difficultyVariance));
       
        return CreateEnemyDataFromRow(rowIndex);
    }
    public GameObject CreateEnemyDataFromRow(int rowIndex)
    {
        var columns = _rows[rowIndex].Split(',');
        
        //1. Create enemy GameObject based on difficulty.
        float difficulty = float.Parse(columns[DIFFICULTY_COLUMN]);
        GameObject enemy;
        
        if (difficulty >= _hardThreshold) {
            enemy = Instantiate(_hardPrefab);
        } 
        else if (difficulty >= _moderateThreshold) {
            enemy = Instantiate(_moderatePrefab);
        } 
        else if (difficulty >= _mildThreshold) {
            enemy = Instantiate(_mildPrefab);
        }
        else {
            enemy = Instantiate(_easyPrefab);
        }
        
        //2. Read and apply unit data
        EnemyUnit unitData = enemy.GetComponent<EnemyUnit>();
        try
        {
            unitData.name = columns[NAME_COLUMN];
            unitData.MaxPriorityThreshold = float.Parse(columns[AI_COLUMN]);
            unitData.C_Heal = float.Parse(columns[AI_COLUMN + 1]);
            unitData.C_Attack = float.Parse(columns[AI_COLUMN + 2]);
            unitData.C_StatMod = float.Parse(columns[AI_COLUMN + 3]);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogError("Could not read enemy data. A column index was out of bounds!");
        }
        
        //3. Try Applying Skills
        try
        {
            for (int i = 0; i < 2; i++)
            {
                if(columns[SKILLS_COLUMN + i] != string.Empty)
                    unitData.TryAddSkill(Resources.Load<Skill>("Skills/" + columns[SKILLS_COLUMN + i]));
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Could not read {unitData.name}'s skills from Resource folder.\n Exception: {e}");
        }

        Debug.Log($"Diff: {LevelManager.Instance.DifficultyValue} ~ Chose {unitData.name} with difficulty {difficulty}");
        return enemy;
    }
}
