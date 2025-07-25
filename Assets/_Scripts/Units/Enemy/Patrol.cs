
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Patrol, Chase, InCombat
}

[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    [Header("Target Detection")]
    [SerializeField] private float _sightDistance = 7f; //how far the sighline raycast will go
    [SerializeField] private float _collisionDistance = 0.7f;
    [SerializeField] private float _spherecastRadius = 1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _chaseSpeed;

    [Header("Patrol")]
    public List<Transform> PatrolNodes;
    [SerializeField] private float _patrolSpeed;

    public NavMeshAgent agent;
    private int _nodeIndex = 0;


    private CharacterController _controller;

    private EnemyState _state;
    GameObject target;
    Vector3 _targetDirection;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<CharacterController>();
        agent.speed = _patrolSpeed;

        _state = EnemyState.InCombat;

        if (PatrolNodes.Count > 0)
            agent.SetDestination(PatrolNodes[0].position);
    }

    void FixedUpdate()
    {
        switch (_state)
        {
            case EnemyState.Chase:
                ChaseBehaviour();
                break;
            case EnemyState.Patrol:
                PatrolBehaviour();
                break;
            case EnemyState.InCombat:
                break;
        }
    }

    public void PatrolBehaviour()
    {
        //Spherecast to check for player
        if (Physics.SphereCast(gameObject.transform.position, _spherecastRadius, transform.forward, out RaycastHit hit, _sightDistance, _layerMask))
        {
            _state = EnemyState.Chase;
            target = hit.transform.gameObject;
            agent.speed = _chaseSpeed;
        } 
        //Patrol
        else if(PatrolNodes.Count > 1 && agent.remainingDistance <= _collisionDistance)
        {
            _nodeIndex = (_nodeIndex + 1) % PatrolNodes.Count;
            agent.SetDestination(PatrolNodes[_nodeIndex].position);
        }
    }

    public void ChaseBehaviour()
    {
        if (Physics.Raycast(gameObject.transform.position, transform.forward, out RaycastHit hit, _collisionDistance, _layerMask)) // check if player is in range
        {
            //ToDo: Attack sequence here;
            CombatManager.Instance.BeginBattle(false);
        }
        else
        {
            agent.SetDestination(target.transform.position);
        }
    }

    public void PatrolToCombat()
    {
        agent.enabled = false;
        _state = EnemyState.InCombat;
    }

    public void SetState(EnemyState state) { this._state = state; }
}
