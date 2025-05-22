using System;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    //private Rigidbody _rb;
    private void Start()
    {
        //_rb = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Battle Start");
        }
    }
}
