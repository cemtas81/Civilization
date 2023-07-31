using System.Collections;

using UnityEngine;

public class SettlementSpawner : MonoBehaviour
{
    public float spawnInterval;
    public int maxSoldier;
    public int soldiers;
    public GameObject prefab;
    private bool isHere;
    private BarbCont2 barbar;
    private Transform player;
    public float dist;
    public bool couldBurn;
    private void Awake()
    {
     
        barbar = SharedVariables.Instance.cont;
        player = barbar.GetComponent<Transform>();
        StartCoroutine(SpawnCoroutine());
    }
 
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (soldiers<=maxSoldier&&isHere)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
                soldiers++;
            }
            
        }
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position ,player.position);
        if (distance<=dist)
        {
            isHere=true;
        }
        else if (soldiers>=maxSoldier)
        {
            couldBurn=true;
        }
        else
        {
            isHere = false;
        }

    }
}
