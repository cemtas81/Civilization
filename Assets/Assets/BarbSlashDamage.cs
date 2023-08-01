
using UnityEngine;

public class BarbSlashDamage : MonoBehaviour
{
    public int damage;
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Enemy":
                BarbEnemyCont enemy = other.GetComponent<BarbEnemyCont>();
                enemy.LoseHealth(damage);
                enemy.BloodParticle(transform.position, rotation);
                break; 
            case "Enemy2":
                BarbEnemyCont2 enemy2 = other.GetComponent<BarbEnemyCont2>();
                enemy2.LoseHealth(damage);
                enemy2.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BarbEnemyCont2 boss = other.GetComponent<BarbEnemyCont2>();
                boss.LoseHealth(damage);
                boss.BloodParticle(transform.position, rotation);
                break;

        }

    }
}
