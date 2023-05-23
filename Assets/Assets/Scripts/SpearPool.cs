using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearPool : MonoBehaviour
{
    public static SpearPool SharedInstance;
    private List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;
    private bool willGrow = true;
    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false);
            pooledObjects.Add(obj);
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
        if (willGrow)
        {
            GameObject obj = Instantiate(objectToPool);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;

    }
}
