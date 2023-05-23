using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Enemy":
                EnemyController enemy = other.GetComponent<EnemyController>();
                enemy.LoseHealth(1);
                enemy.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BossCont2 boss = other.GetComponent<BossCont2>();
                boss.LoseHealth(1);
                boss.BloodParticle(transform.position, rotation);
                break;
            case "Finish":
                //Instantiate the prefab at the position of the bomb
                _ = Instantiate(explosionPrefab, transform.position,
                    Quaternion.identity);
                Destroy(gameObject);
                break;              
        }
 
    }
}
