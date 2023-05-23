using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling2 : MonoBehaviour
{
    public static ObjectPooling2 SharedInstance;
    public Dictionary<GameObject, List<GameObject>> pooledObjects;
    public List<GameObject> objectsToPool;
    public List<int> amountToPool;

    private void Awake()
    {
        SharedInstance = this;
    }

    private void Start()
    {
        pooledObjects = new Dictionary<GameObject, List<GameObject>>();
        for (int i = 0; i < objectsToPool.Count; i++)
        {
            pooledObjects[objectsToPool[i]] = new List<GameObject>();
            for (int j = 0; j < amountToPool[i]; j++)
            {
                GameObject obj = Instantiate(objectsToPool[i]);
                obj.SetActive(false);
                pooledObjects[objectsToPool[i]].Add(obj);
            }
        }
    }

    public GameObject GetPooledObject(GameObject prefab)
    {
        int index = objectsToPool.IndexOf(prefab);
        if (index == -1)
        {
            Debug.LogError("Prefab not found in the list of objects to pool.");
            return null;
        }

        for (int i = 0; i < pooledObjects[prefab].Count; i++)
        {
            if (!pooledObjects[prefab][i].activeInHierarchy)
            {
                return pooledObjects[prefab][i];
            }
        }

        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);
        pooledObjects[prefab].Add(obj);
        return obj;
    }
}
