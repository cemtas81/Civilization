using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbAid : MonoBehaviour
{
    private int healAmount = 15;
    private int lifeTime = 5;
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<BarbCont2>().HealHealth(healAmount);
            Destroy(gameObject);
        }
    }
}
