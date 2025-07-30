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
    private const int NAME_ROW = 1;
    private const int SKILLS_START_ROW = 6;
    private const int AI_START_ROW = 8;
    
    private string _fileContents;
    private string[] _rows;
    
    #endregion
    public override void Awake()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(FILE_NAME);
        
        if (csvFile != null)            //Read the enemy info file
        {
            _fileContents = csvFile.text;
            _rows = _fileContents.Split('\n', StringSplitOptions.RemoveEmptyEntries);
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
        GameObject enemy = Instantiate(_easyPrefab);    //Temp ToDo: separate the different prefabs based on the read difficulty threshold
        EnemyUnit unitData = enemy.GetComponent<EnemyUnit>();
        
        //Parse the information of one row and put the read data into the specified enemy unit
        unitData.name = columns[NAME_ROW];
        
        //not done here
        
        //disable gameobject and place in combatManager enemy list > this goes in whatever script is creating the object.
        return enemy;
    }
}
