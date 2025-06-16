using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Sightline : MonoBehaviour
{           
    [SerializeField] private float _sightDistance = 7f; ///how far the sighline raycast will go
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _chaseSpeed;
    [SerializeField] private float _detectionRadius;

    private CharacterController _controller;
 
    private bool _spottedTarget;
    GameObject target;
    Vector3 _targetDirection;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        if (CombatManager.Instance.InCombat())
            return;

        //Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.red);
        if (_spottedTarget)
        {
            if (Physics.Raycast(gameObject.transform.position, transform.forward, out RaycastHit hit, _detectionRadius, _layerMask))
            {
                    CombatManager.Instance.BeginBattle();
            } else
            {
                _targetDirection = (target.transform.position - transform.position).normalized;
                _targetDirection.y = 0f;

                transform.LookAt(target.transform.position);
                _controller.Move(_targetDirection * _chaseSpeed * Time.deltaTime);
            }

        } 
        else if (Physics.Raycast(gameObject.transform.position, transform.forward, out RaycastHit hit, _sightDistance, _layerMask))
        {
            _spottedTarget = true;
            target = hit.transform.gameObject;
        }

    }
}
