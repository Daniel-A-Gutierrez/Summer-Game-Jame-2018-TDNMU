using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public List<GameObject> pooledObjects;
    public BulletPoolType poolType;
    public int amountToPool;
    public bool shouldExpand;

    private void OnEnable()
    {
        poolType.Attach(this);
    }

    private void OnDisable()
    {
        poolType.Detach();
    }

    private GameObject NewObject()
    {
        GameObject obj = (GameObject)Instantiate(poolType.bullet, transform);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }

    // Use this for initialization
    void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            NewObject();
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (shouldExpand)
        {
            return NewObject();
        }
        return null;
    }
}
