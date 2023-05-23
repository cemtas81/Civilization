using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Player":
                PlayerController oyuncu = other.GetComponent<PlayerController>();
                //oyuncu.Upgrade(1); 
                oyuncu.Spell(1);
                //enemy.BloodParticle(transform.position, rotation);
                Destroy(gameObject);
                break;
           
        }

    }
}
