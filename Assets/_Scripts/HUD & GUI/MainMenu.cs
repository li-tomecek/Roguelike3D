using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _continueGameButton;

    public void Awake()
    {
        _continueGameButton.interactable = SaveManager.Instance.CanFindSaveData();
    }

    public void StartNewGame()
    {
        LevelManager.Instance.LoadLevel(LevelManager.Instance.GetRandomPlayableLevelIndex());
    }

    public void ResumeGame()
    {
        SaveManager.Instance.LoadGame();
    }
    
    
    public void ExitGame()
    {
        Application.Quit();
    }
}