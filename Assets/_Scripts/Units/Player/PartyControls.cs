using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles logic related to the entire party:
 *  - Handles input for out-of-combat controls and has last arty memebers follow the first party member
 * 
 */

public class PartyControls : MonoBehaviour
{
    public static PartyControls Instance;
    [SerializeField] private List<PlayerUnit> _partyMembers = new List<PlayerUnit>();

    [Header("Party Movement")]
    [SerializeField] private float _jumpDelay;
    private List<PlayerMovement> _partyMovement = new List<PlayerMovement>();

    private bool _following = false;

    [Header("Ranged Attack")]
    [SerializeField] GameObject _projectilePrefab;
    [SerializeField] float _projectileCooldown = 0.5f;
    private float _timeLastFired = 0;
    private int MeleeHash = Animator.StringToHash("MeleeAttack");


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
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

        _partyMovement[0].gameObject.layer = LayerMask.NameToLayer("ControlledPlayer");     //so changing character order in the editor will automatically set the controller player
    }
    private void Update()
    {
        Debug.DrawRay(_partyMembers[0].gameObject.transform.position, _partyMembers[0].transform.forward * 2f);
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
        
        _partyMembers[0].gameObject.GetComponent<Animator>().SetTrigger(MeleeHash);     //TODO: change this to a spell-cast animation
        
        //Fire a projectile
        Instantiate(_projectilePrefab, 
            _partyMovement[0].GetProjectileOrigin().position, 
            _partyMovement[0].GetProjectileOrigin().rotation);  
    }

    private void CastForInteract()
    {     
        if (Physics.Raycast(_partyMovement[0].GetProjectileOrigin().position, _partyMembers[0].transform.forward, out RaycastHit hit, 2.5f))
        {
            if (hit.transform.gameObject.GetComponent<IInteractable>() != null)
            {
                hit.transform.gameObject.GetComponent<IInteractable>().Interact();
            }
        }
    }
    public List<PlayerUnit> GetPartyMembers() { return _partyMembers; }
}
