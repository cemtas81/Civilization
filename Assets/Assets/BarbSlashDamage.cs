
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
            case "Boss":
                BarbEnemyCont boss = other.GetComponent<BarbEnemyCont>();
                boss.LoseHealth(damage);
                boss.BloodParticle(transform.position, rotation);
                break;

        }

    }
}
