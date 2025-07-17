using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private IState _currentState;
    [SerializeField] private GameObject _party;

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(_party.gameObject);   //Party information should persist between scenes
    }

    public void Update()
    {
        _currentState?.Update();
    }
    
    public void ChangeState(IState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }
}
