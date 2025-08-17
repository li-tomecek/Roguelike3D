using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] float _travelSpeed = 10f;
    [SerializeField] float _timeToDie = 2f;
    void Start()
    {
    }

    private void FixedUpdate()
    {
        _timeToDie -= Time.deltaTime;
        if (_timeToDie <= 0)
            Destroy(gameObject);

        transform.Translate(Vector3.forward * _travelSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //ignore
            return;
        }
        
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (CombatManager.Instance.InCombat() == false)
                CombatManager.Instance.BeginBattle(true);
        }

        Destroy(gameObject);
    }
}
