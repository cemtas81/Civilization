
using UnityEngine;

public class Bombing : MonoBehaviour
{
   
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
          
        }

    }
}
