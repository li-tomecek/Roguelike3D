using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private List<PlayerMovement> _partyMovement = new List<PlayerMovement>();
    [SerializeField] private float _jumpDelay;

    private bool _following = false;
    private void OnEnable()
    {
        if (gameObject.GetComponent<InputController>() != null)
        {   
            gameObject.GetComponent<InputController>().MoveEvent += HandleMoveInput;
            gameObject.GetComponent<InputController>().JumpEvent += HandleJump;
        }
    }

    // PARTY MOVEMENT
    private void HandleMoveInput(Vector2 input)
    {
        _partyMovement[0].SetDirectionalInput(input);
        if (input == Vector2.zero)
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
            _partyMovement[0].SetDirectionalInput(input);
        }
    }

    private void Update()
    {
        if (!_following)
            return;
        
        for (int i = 1; i < _partyMovement.Count; i++)
        {
            _partyMovement[i].SetDirectionalInput(_partyMovement[i - 1].transform.Find("FollowerPoint").position - _partyMovement[i].transform.position);
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
    
    
}
