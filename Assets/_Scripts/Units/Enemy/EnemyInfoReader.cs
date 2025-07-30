using UnityEngine;

public class EnemyInfoReader : Singleton<EnemyInfoReader>
{
    public const string FILE_NAME = "EnemyInfo";
    private string _fileContents;
    
    public override void Awake()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(FILE_NAME);
        
        if (csvFile != null)            //Read the enemy info file
        {
            _fileContents = csvFile.text;
            Debug.Log(_fileContents);
        }
        else
        {
            Debug.LogError($"There is no file \"{FILE_NAME}\" in Resources folder.");
        }
        
    }

    public void ReadEnemyDataFromRow(EnemyUnit unit, int rowIndex)
    {
        //Parse the information of one row and put the read data into the specified enemy unit
    }
}
