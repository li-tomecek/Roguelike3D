using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isLocked = false;
    [SerializeField] Transform pivot;
    [SerializeField] float openRotationSpeed = 60f;
    [SerializeField] float openRotation = 90f;

    public void Interact()
    {
        if (_isLocked)
        {
            Debug.Log("The door is locked.");
            //ToDo: Make the door shake or give some sort of visual feedback to the player that it is locked.
            return;
        }
        
        StartCoroutine(OpenDoor());
    }
    public IEnumerator OpenDoor()
    {
        float totalRotation = 0;
        while(totalRotation < openRotation)
        {
            transform.RotateAround(pivot.position, Vector3.up, openRotationSpeed * Time.deltaTime);
            totalRotation += openRotationSpeed * Time.deltaTime;
            yield return null;
        } 
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    
    public void UnlockDoor()
    {
        _isLocked = false;
    }
}
