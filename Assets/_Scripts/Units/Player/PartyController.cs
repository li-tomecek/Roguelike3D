using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Windows;

/* Handles logic related to the entire party:
 *  - Handles input for out-of-combat controls and has last party memebers follow the first party member
 *  - Handles save-state logic for the player units
 */

public class PartyController : Singleton<PartyController>, ISaveable
{
    #region
    [SerializeField] private List<PlayerUnit> _partyMembers = new List<PlayerUnit>();
  
    [Header("Party Movement")]
    [SerializeField] private float _jumpDelay;
    private List<PlayerMovement> _partyMovement = new List<PlayerMovement>();
    private PlayerMovement _partyLeader;


    private bool _following = false;

    [Header("Ranged Attack")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] float _projectileCooldown = 0.5f;
    private float _timeLastFired = 0;
    #endregion
    private void Start()
    {
       
        foreach(PlayerUnit unit in _partyMembers)
        {
            _partyMovement.Add(unit.gameObject.GetComponent<PlayerMovement>());
        }

        if (InputController.Instance != null)
        {   
            InputController.Instance.MoveEvent += HandleMoveInput;
            InputController.Instance.JumpEvent += HandleJump;
            InputController.Instance.AttackEvent += FireProjectile;
            InputController.Instance.InteractEvent += CastForInteract;
        }
        else
        {
            Debug.LogWarning("No InputController found");
        }

        _partyLeader = _partyMovement[0];
        _partyLeader.gameObject.layer = LayerMask.NameToLayer("ControlledPlayer");     //so changing character order in the editor will automatically set the controlled player
        SetupHUD();
    }
    private void SetupHUD()
    {
        if(GetComponentInChildren<Party_HUD>() != null)
        {
            Party_HUD HUD = GetComponentInChildren<Party_HUD>();
            foreach (PlayerUnit unit in _partyMembers)
            {
                HUD.SetupElement(unit);
            }
        }
    }
    private void Update()
    {
        if (_following)
        {
            for (int i = 1; i < _partyMovement.Count; i++)
            {
                _partyMovement[i].SetDirectionalInput(_partyMovement[i - 1].GetFollowerPoint().position - _partyMovement[i].transform.position);
            }
        }
    }
    
    // --- PARTY OUT-OF-COMBAT CONTROLS --- 
    // ------------------------------------
    #region
    private void HandleMoveInput(Vector2 input)
    {
        _partyLeader.SetDirectionalInput(input);   //only the first party member is directly controlled via input
        
        if (input == Vector2.zero)                      //if the player is not moving
        {
            _following = false;
            foreach (PlayerMovement pm in _partyMovement)
            {
                pm.SetDirectionalInput(input);
            }
        }
        else
        {
            _following = true;
        }
    }
    private void HandleJump()
    {
        StartCoroutine(StartJumps());
    }
    private IEnumerator StartJumps()
    {
        for (int i = 0; i < _partyMovement.Count; i++)
        {
            _partyMovement[i].HandleJump();
            yield return new WaitForSeconds(_jumpDelay);

        }
    }
    private void FireProjectile()
    {
        if (Time.time - _timeLastFired < _projectileCooldown)
            return;
        
        _timeLastFired = Time.time;

        _partyLeader.GetComponent<PlayerAnimator>().PlayMagicAnimation();
        
        //Fire a projectile
        Instantiate(_projectilePrefab, 
            _partyLeader.GetProjectileOrigin().position, 
            _partyLeader.GetProjectileOrigin().rotation);  
    }
    private void CastForInteract()
    {
        Vector3 start = _partyLeader.GetProjectileOrigin().position;
        start.z = _partyLeader.transform.position.z;    // move it back
        
        if (Physics.Raycast(start, _partyLeader.transform.forward, out RaycastHit hit, 2.5f))
        {
            if (hit.transform.gameObject.GetComponent<IInteractable>() != null)
            {
                hit.transform.gameObject.GetComponent<IInteractable>().Interact();
            }
        }
    }

    public IEnumerator SetPartyDirectionForDuration(Vector2 directionalInput, float duration)
    {
        _partyLeader.SetDirectionalInput(directionalInput);
        _following = true;
        
        yield return new WaitForSeconds(duration);
        
        foreach (PlayerMovement mv in _partyMovement)
        {
            mv.SetDirectionalInput(Vector2.zero);
        }
        _following = false;
    }
    
    
    #endregion
    
    // --- State Saving --- 
    // --------------------
    #region
    public object CaptureState()
    {
        PartyData partyData = new PartyData();
        foreach(PlayerUnit unit in _partyMembers)
        {
            PlayerUnitData data = new PlayerUnitData(unit.GetStats(), unit.GetHealth(), unit.GetSkills());
            partyData.PartyUnits.Add(data);
        } 
        return partyData;
    }

    public void RestoreState(object data)
    {
        try
        {
            PartyData partyData = (PartyData)data;
            for(int i = 0; i < partyData.PartyUnits.Count; i++)
            {
                _partyMembers[i].SetStats(partyData.PartyUnits[i].Stats);
                _partyMembers[i].SetHealth(partyData.PartyUnits[i].CurrentHealth);
                _partyMembers[i].SetSkills(partyData.PartyUnits[i].Skills);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
        
        //Set player at level start position
    }
    #endregion
    
    public List<PlayerUnit> GetPartyMembers() { return _partyMembers; }
}
