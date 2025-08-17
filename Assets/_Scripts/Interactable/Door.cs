using System.Collections;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private bool _isLocked = true;
    private PlayerUnit _rewardedUnit;
    [SerializeField] private TextMeshProUGUI _unitNameTxt;


    [Header("Animation")]
    [SerializeField] Transform pivot;
    [SerializeField] float openRotationSpeed = 60f;
    [SerializeField] float openRotation = 90f;


    public void Start()
    {
        _rewardedUnit = PartyController.Instance.GetPartyMembers()[Random.Range(0, PartyController.Instance.GetPartyMembers().Count)];
        _unitNameTxt.text = _rewardedUnit.name;
        _unitNameTxt.enabled = false;
    }
    public void Interact()
    {
        if (!_isLocked)
        {
            LevelManager.Instance.RewardedUnit = _rewardedUnit;
            StartCoroutine(OpenAndEnterDoor());
        }
    }
    public IEnumerator OpenAndEnterDoor()
    {
        InputController.Instance.DisableActiveMap();
        
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
    }
    public void UnlockDoor()
    {
        _isLocked = false;
        _unitNameTxt.enabled = true;

    }
}
