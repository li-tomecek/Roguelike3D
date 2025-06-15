using UnityEngine;

public class Sightline : MonoBehaviour
{           
    [SerializeField] private float _sightDistance = 7f; ///how far the sighline raycast will go
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _chaseSpeed;
    private bool _spottedTarget;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (CombatManager.Instance.InCombat())
            return;
        
        //Debug.DrawRay(transform.position, transform.forward * _sightDistance, Color.red);
        
        if (_spottedTarget) //chase the target
        {
            transform.position = Vector3.MoveTowards(transform.position, CombatManager.Instance.GetPlayerUnits()[0].transform.position, _chaseSpeed * Time.deltaTime);
            transform.LookAt(CombatManager.Instance.GetPlayerUnits()[0].transform.position);
        }
        else if (Physics.Raycast(gameObject.transform.position, transform.forward, out RaycastHit hit, _sightDistance, _layerMask))
        {
            _spottedTarget = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!CombatManager.Instance.InCombat() && (1 << other.gameObject.layer) == _layerMask)
        {
            Debug.Log("eyooo");
            CombatManager.Instance.BeginBattle();
        }
    }
}
