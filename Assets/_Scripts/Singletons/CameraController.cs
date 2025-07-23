using Unity.Cinemachine;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [SerializeField] private CinemachineCamera _topDownCamera;
    [SerializeField] private CinemachineCamera _combatCamera;

    public void ToggleCombatCamera()
    {
        if (_topDownCamera.enabled)
        {
            _topDownCamera.enabled = false;
            _combatCamera.enabled = true;

        }
        else
        {
            _topDownCamera.enabled = true;
            _combatCamera.enabled = false;
        }
    }
    
    public void EnableCombatCamera()
    {
        _topDownCamera.enabled = false;
        _combatCamera.enabled = true;
        Debug.Log("combat on");
    }
    
    public void DisableCombatCamera()
    {
        Debug.Log("combat off"); 
        _topDownCamera.enabled = true; 
        _combatCamera.enabled = false;
    }
}
