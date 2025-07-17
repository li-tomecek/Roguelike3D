using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private IState _currentState;
    [SerializeField] private GameObject _party;
    
    //Maybe include LevelManager in here?
    //Consider whether LevelManager, CombatManager, and CombatInterface ahould all be Singletons

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_party.gameObject);   //Party information should persist between scenes
    }
    
    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
    
    
    
    
}
