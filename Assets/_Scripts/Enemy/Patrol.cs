
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private List<Transform> _patrolNodes;
    [SerializeField] private float _patrolSpeed;

    private NavMeshAgent _agent;
    private int _nodeIndex = 0;


    private CharacterController _controller;
 
    private bool _spottedTarget;
    GameObject target;
    Vector3 _targetDirection;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _controller = GetComponent<CharacterController>();
        _agent.speed = _patrolSpeed;

        if (_patrolNodes.Count > 0)
            _agent.SetDestination(_patrolNodes[0].position);
    }

    void FixedUpdate()
    {
        if (CombatManager.Instance.InCombat())
        {
            _agent.enabled = false; //disable the NavMeshAgent   
            return;
        }

        if (_spottedTarget) //chasing the target
        {
            if (Physics.Raycast(gameObject.transform.position, transform.forward, out RaycastHit hit, _collisionDistance, _layerMask))
            {
                CombatManager.Instance.BeginBattle(false);
            } else
            {
                _agent.SetDestination(target.transform.position);
            }

        } //Check for the target
        else if (Physics.SphereCast(gameObject.transform.position, _spherecastRadius, transform.forward, out RaycastHit hit, _sightDistance, _layerMask))
        {
            _spottedTarget = true;
            target = hit.transform.gameObject;
            _agent.speed = _chaseSpeed;
        } 
        else if(_patrolNodes.Count > 1 && _agent.remainingDistance <= _collisionDistance) //patrol
        {
            _nodeIndex = (_nodeIndex + 1) % _patrolNodes.Count;
            _agent.SetDestination(_patrolNodes[_nodeIndex].position);
        }

    }
}
