

using UnityEngine;

public class SpearDamage : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerMovement plyrmvmnt;
    private void Awake()
    {
        plyrmvmnt = FindObjectOfType<PlayerMovement>();
        Vector3 aim2 = plyrmvmnt.aim.position;

        // Calculate the direction from the spear's position to the aim position
        Vector3 direction = (aim2 - transform.position).normalized;

        // Apply a force in the calculated direction
        rb = GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(direction.x,0,direction.z) * 75f, ForceMode.Impulse);

        Invoke(nameof(Dest), 1.5f);
    }
    
    void OnTriggerEnter(Collider other)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        switch (other.tag)
        {
            case "Enemy":
                BarbEnemyCont enemy = other.GetComponent<BarbEnemyCont>();
                enemy.LoseHealth(2);
                enemy.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BossCont2 boss = other.GetComponent<BossCont2>();
                boss.LoseHealth(2);
                boss.BloodParticle(transform.position, rotation);
                break;
        }

    }
   void Dest()
    {
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
