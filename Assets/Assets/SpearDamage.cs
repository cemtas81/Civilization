

using UnityEngine;

public class SpearDamage : MonoBehaviour
{
    //private Rigidbody rb;
    private Transform aim2;
    private PlayerMovement plyrmvmnt;
    private Vector3 targetPosition;
    public float movementSpeed;
    private void Start()
    {
        plyrmvmnt = FindObjectOfType<PlayerMovement>();
        aim2 = plyrmvmnt.aim;
        targetPosition = aim2.position;
        Invoke(nameof(Dest), 1f);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * movementSpeed);
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

                default:
                Destroy(this.gameObject);
                break;

        }

    }
   void Dest()
    {
        Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
