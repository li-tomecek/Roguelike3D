using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            CameraController.Instance.ToggleCombatCamera();
            //Debug.Log("Battle Start");
        }
    }
}
