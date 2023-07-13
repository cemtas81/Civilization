using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
public class MySolidSpawner : MonoBehaviour
{
 
    public GameObject[] prefabs;
    public int minSpawnCount = 5, maxSpawnCount = 10, maxActivePrefabs = 65;
    public GameObject movableObject;
    [Range(0.0f, 1f)]
    public float nadide;
    public List<GameObject> spawnedPrefabs;
    public float spawnInterval,spawnIntervalBoss, desiredCircleRadius; 
    public bool BossHere;
    private BarbScreenCont screen;
    private ObjectPooling2 pooling2;
    void Start()
    {
        // Initialize the list of spawned prefabs
        spawnedPrefabs = new List<GameObject>();
        Spawn4();
        StartCoroutine(SpawnCoroutine());
        StartCoroutine(SpawnCoroutineBoss());
        screen=SharedVariables.Instance.screenCont;
        pooling2=ObjectPooling2.SharedInstance;
    }

    IEnumerator SpawnCoroutine()
    {
        
       
            // Loop indefinitely
            while (true)
            {
                yield return new WaitForSeconds(spawnInterval);

                int spawnCount = Random.Range(minSpawnCount, maxSpawnCount + 1);
                for (int i = 0; i < spawnCount; i++)
                {
                    if (BossHere==false)
                    {
                        Spawn4();
                    }               
                }             
            }
        
    }
    IEnumerator SpawnCoroutineBoss()
    {
        while (true)
        {
            if (BossHere == false)
            {
                yield return new WaitForSeconds(spawnIntervalBoss);

                BossSpawn();
                BossHere = true;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void Spawn3(Vector3 pos)
    {

        GameObject prefabToSpawn = pooling2.GetPooledObject(pooling2.objectsToPool[3]);
        if (prefabToSpawn==null) return;      
        prefabToSpawn.transform.SetPositionAndRotation(new Vector3(pos.x,1,pos.z), Quaternion.identity);
    
        prefabToSpawn.SetActive(true);

    }
    public void Spawn4()
    {
        GameObject prefab;
        float randomValue = Random.value;
        if (randomValue <= nadide)
        {
            prefab = prefabs[0];  // the first prefab has a higher chance of spawning 
        }
        else if (randomValue <= 0.95f)
        {
            prefab = prefabs[1];  // the second prefab has a lower chance of spawning
        }
        else
        {
            prefab = prefabs[2];  // the third prefab has an even lower chance of spawning
        }

        if (spawnedPrefabs.Count >= maxActivePrefabs)
        {
            return;
        }
        float angle = Random.Range(0f, 360f);
        float x = movableObject.transform.position.x + desiredCircleRadius * Mathf.Cos(angle);
        //float y = movableObject.transform.position.y;
        float z = movableObject.transform.position.z + desiredCircleRadius * Mathf.Sin(angle);
        Vector3 position = new Vector3(x, 0, z);
        prefab.transform.SetPositionAndRotation(position, Quaternion.identity);
        Instantiate(prefab);

    }
    public void BossSpawn()
    {
        int prefabIndex = Random.Range(0, 2); // choose either 0 or 1
        GameObject prefab = prefabs[prefabIndex + 3]; // add 4 to get prefab 4 or 5

        float angle = Random.Range(0f, 360f);
        float x = movableObject.transform.position.x + desiredCircleRadius* Random.Range(5.5f, 7) * Mathf.Cos(angle);
        screen.ShowBossText();
        float z = movableObject.transform.position.z + desiredCircleRadius *Random.Range(5.5f,7) * Mathf.Sin(angle);
        Vector3 position = new Vector3(x, 0, z);
        prefab.transform.SetPositionAndRotation(position, Quaternion.identity);
        Instantiate(prefab);
    }
}


