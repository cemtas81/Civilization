using UnityEngine;
using DG.Tweening;

public class SpearDamage : MonoBehaviour
{
    private Transform aim2;
    private PlayerMovement plyrmvmnt;
    private Vector3 targetPosition;
    private Rigidbody rb;

    private void Start()
    {
        plyrmvmnt = FindObjectOfType<PlayerMovement>();
        aim2 = plyrmvmnt.aim;
        targetPosition = aim2.position;
        Invoke(nameof(Dest), 1f);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        transform.DOMove(targetPosition, 0.3f).SetEase(Ease.Linear)
        .OnComplete(() => rb.isKinematic = false);
      
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
    }
}
