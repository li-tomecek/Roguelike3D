using UnityEngine;

public class CombatState : IState
{
    public void Enter()
    {
        //CombatManager.BeginBattle() does most logic rated to setup
       
        CameraController.Instance.ToggleCombatCamera();
        InputController.Instance.ActivateMenuMap();
    }

    public void Update() { }
    
    public void Exit()
    {
        CameraController.Instance.ToggleCombatCamera();
    }
}

public class PreCombatState : IState
{
    public void Enter() { }
    public void Exit() { } 
    public void Update() { }

}

public class PostCombatState : IState
{
    public void Enter() { }
    public void Exit() { } 
    public void Update() { }

}