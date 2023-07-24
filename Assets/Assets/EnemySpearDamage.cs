using UnityEngine;
using DG.Tweening;

public class EnemySpearDamage : MonoBehaviour
{
 
    private Vector3 targetPosition;
    private Rigidbody rb;

    private void Start()
    {
  
        targetPosition = SharedVariables.Instance.playa.transform.position;
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
   
        if (other.CompareTag("Player"))
        {
            BarbCont2 enemy = other.GetComponent<BarbCont2>();
            enemy.LoseHealth(5);
            Dest();
        }
        if (other.CompareTag("Enemy"))
        {
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            //GetComponent<BoxCollider>().enabled = false;
            GetComponent<Collider>().isTrigger = false;
        }
    }

    void Dest()
    {
        Destroy(this.gameObject);
    }
}
