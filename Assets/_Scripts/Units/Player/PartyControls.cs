using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles logic related to the entire party:
 *  - Handles input for out-of-combat controls and has last arty memebers follow the first party member
 * 
 */

public class PartyControls : MonoBehaviour
{
    [Header("Party Movement")]
    [SerializeField] private List<PlayerMovement> _partyMovement = new List<PlayerMovement>();
    [SerializeField] private float _jumpDelay;
    private bool _following = false;

    [Header("Ranged Attack")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] float _projectileCooldown = 0.5f;
    private float _timeLastFired = 0;
   
    private void Start()
    {
        if (InputController.Instance != null)
        {   
            InputController.Instance.MoveEvent += HandleMoveInput;
            InputController.Instance.JumpEvent += HandleJump;
            InputController.Instance.AttackEvent += FireProjectile;
        }
        else
        {
            Debug.LogWarning("No InputController found");
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

    // PARTY MOVEMENT AND OUT-OF-COMBAT CONTROLS
    private void HandleMoveInput(Vector2 input)
    {
        _partyMovement[0].SetDirectionalInput(input);   //only the first party member is directly controlled via input
        if (input == Vector2.zero)      //if the player is not moving
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
        
        //Fire a projectile
        Instantiate(_projectilePrefab, 
            _partyMovement[0].GetProjectileOrigin().position, 
            _partyMovement[0].GetProjectileOrigin().rotation);  
    }
}
