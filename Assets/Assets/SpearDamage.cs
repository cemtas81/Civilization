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

        plyrmvmnt=SharedVariables.Instance.plyrmvmnt;
        aim2 = plyrmvmnt.aim;
        targetPosition = aim2.position;
        Invoke(nameof(Dest), 1.5f);
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;      
        float distance = Vector3.Distance(transform.position, targetPosition);
        float duration = distance / 25;
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear)
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
            case "Enemy2":
                BarbEnemyCont2 enemy2 = other.GetComponent<BarbEnemyCont2>();
                enemy2.LoseHealth(2);
                enemy2.BloodParticle(transform.position, rotation);
                break;
            case "Boss":
                BarbEnemyCont boss = other.GetComponent<BarbEnemyCont>();
                boss.LoseHealth(2);
                boss.BloodParticle(transform.position, rotation);
                Dest();
                break;
            //default:
            //    GetComponent<Collider>().isTrigger= false;
            //    break;
        }
    }

    void Dest()
    {
        Destroy(this.gameObject);
    }
}
