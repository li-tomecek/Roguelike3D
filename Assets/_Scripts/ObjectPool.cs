using System.Collections.Generic;
using UnityEngine;

//Got skeleton code from:
//https://learn.unity.com/tutorial/introduction-to-object-pooling

public class ObjectPool
{
    private List<GameObject> _pool;

    public ObjectPool(GameObject _objectType, int amount)
    {
        _pool = new List<GameObject>();
        GameObject tmp;
        
        for (int i = 0; i < amount; i++)
        {
            tmp = GameObject.Instantiate(_objectType);
            tmp.SetActive(false);
            _pool.Add(tmp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                return _pool[i];
            }
        }
        return null;
    }

    public GameObject GetActivePooledObject()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeInHierarchy)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }
        return null;
    }

}
