using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public static CameraController Instance;

    [SerializeField] private CinemachineCamera _topDownCamera;
    [SerializeField] private CinemachineCamera _combatCamera;


    public void Awake()     //Singleton, but we do want to destoy on load so that each scene has its own custom combat camera setup
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
        }   
    }


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
    }
    
    public void DisableCombatCamera()
    {
        _topDownCamera.enabled = true; 
        _combatCamera.enabled = false;
    }
}
