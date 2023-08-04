using System.Collections;
using System.Collections.Generic;
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
    private List<SettlementSpawner> spawns;
    private bool burnCalled = false;
    public GameObject fire;
    private Target parent;
    private void Awake()
    {
        SharedVariables.Instance.settlementSpawner.Add(this);
        barbar = SharedVariables.Instance.cont;
        player = barbar.GetComponent<Transform>();
        StartCoroutine(SpawnCoroutine());
        spawns=SharedVariables.Instance.settlementSpawner;
        parent=GetComponentInParent<Target>();  
    }
 
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (soldiers<maxSoldier&&isHere)
            {
                Instantiate(prefab, transform.position, Quaternion.identity);
                soldiers++;
                if (soldiers == maxSoldier)
                {
                    couldBurn = true;
                   
                }
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
        else
        {
            isHere = false;
        }
        if (AllElementsCouldBurn()&& !burnCalled)
        {
            Burn();
            burnCalled = true;
        }
    }
    private bool AllElementsCouldBurn()
    {
        foreach (SettlementSpawner spawner in spawns)
        {
            if (!spawner.couldBurn)
            {
                return false;
            }
        }
        return true;
    }

    private void Burn()
    {
        StartCoroutine(parent.Destruction());
        fire.SetActive(true);
    }
  
}
