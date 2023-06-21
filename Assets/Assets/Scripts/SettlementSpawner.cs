using System.Collections;
using Unity.VisualScripting;
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
    // Start is called before the first frame update
    private void Awake()
    {
        barbar = FindObjectOfType<BarbCont2>();
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
    }
}
