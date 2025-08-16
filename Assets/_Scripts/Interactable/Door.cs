using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isLocked = false;
    private PlayerUnit _rewardedUnit;

    [Header("Animation")]
    [SerializeField] Transform pivot;
    [SerializeField] float openRotationSpeed = 60f;
    [SerializeField] float openRotation = 90f;


    public void Start()
    {
        _rewardedUnit = PartyController.Instance.GetPartyMembers()[Random.Range(0, PartyController.Instance.GetPartyMembers().Count)];
    }
    public void Interact()
    {
        if (!_isLocked)
        {
            StartCoroutine(OpenAndEnterDoor());
        }
    }
    public IEnumerator OpenAndEnterDoor()
    {
        //disable controls
        InputController.Instance.DisableAllInputMaps();
        
        yield return OpenDoor();
        
        Vector3 movePosition = transform.position;
        movePosition.z += 2f;
        yield return PartyController.Instance.SetPartyDirectionForDuration(Vector2.up, 1);
        LevelManager.Instance.LoadLevel(LevelManager.Instance.GetRandomPlayableLevelIndex());

        InputController.Instance.ActivateMovementMap();

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
