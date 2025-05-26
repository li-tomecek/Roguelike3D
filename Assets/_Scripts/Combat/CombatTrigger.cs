using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    private bool _triggered;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!_triggered && other.gameObject.tag == "Player")
        {
            CameraController.Instance.ToggleCombatCamera();
            _triggered = true;      //alternatively, just delete the trigger

           CombatManager.Instance.BeginBattle();
        }
    }
}
