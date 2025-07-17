using UnityEngine;

public interface IState
{
    public void Enter();

    public void Update();       //Not to be confused with Unity's update
    public void Exit();
}
