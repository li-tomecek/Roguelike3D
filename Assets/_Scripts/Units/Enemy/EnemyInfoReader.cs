using System;
using UnityEngine;

public class EnemyInfoReader : Singleton<EnemyInfoReader>
{
    #region Fields and Properties
    [Header("Enemy Prefabs")] 
    [SerializeField] private GameObject _easyPrefab;
    [SerializeField] private GameObject _mildPrefab;
    [SerializeField] private GameObject _moderatePrefab;
    [SerializeField] private GameObject _hardPrefab;
    
    [Header ("Difficulty Thresholds")]      //ToDo: Either read these values from file, or write to file so they dont both have to be adjustsed
    [SerializeField] private float _easyThreshold = 0f;
    [SerializeField] private float _mildThreshold = 0.2f;
    [SerializeField] private float _moderateThreshold = 0.6f;
    [SerializeField] private float _hardThreshold = 1f;
    
    //Constants -- these change only if csv file itself changes
    private const string FILE_NAME = "EnemyInfo";
    private const int DIFFICULTY_COLUMN = 0;
    private const int NAME_COLUMN = 1;
    private const int SKILLS_COLUMN = 6;
    private const int AI_COLUMN = 8;
    
    private string[] _rows;
    
    #endregion
    public override void Awake()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(FILE_NAME);
        
        if (csvFile != null)            //Read the enemy info file
        {
            _rows = csvFile.text.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            foreach (string row in _rows)
            {
                Debug.Log(row);
            }
        }
        else
        {
            Debug.LogError($"There is no file \"{FILE_NAME}\" in Resources folder.");
        }
        
    }

    public GameObject CreateEnemyDataFromRow(int rowIndex)
    {
        var columns = _rows[rowIndex].Split(',');
        
        //1. Create enemy GameObject based on difficulty prefab.
        float difficulty = float.Parse(columns[DIFFICULTY_COLUMN]);
        GameObject enemy;
        
        if (difficulty <= _easyThreshold) {
            enemy = Instantiate(_easyPrefab);
        } 
        else if (difficulty <= _mildThreshold) {
            enemy = Instantiate(_mildPrefab);
        } 
        else if (difficulty <= _moderateThreshold) {
            enemy = Instantiate(_moderatePrefab);
        }
        else {
            enemy = Instantiate(_hardPrefab);
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
        //3. Assign the specified skills to the enemy
        // = Resources.Load<TextAsset>(FILE_NAME);
        return enemy;
    }
}
