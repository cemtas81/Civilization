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
        targetPosition.y = 1;
        float duration = distance / 12;
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear)
            .OnComplete(() => rb.isKinematic=false);
    }

    void OnTriggerEnter(Collider other)
    {
   
        if (other.CompareTag("Player"))
        {
            BarbCont2 enemy = other.GetComponent<BarbCont2>();
            enemy.LoseHealth(5);
            Dest();
        }
        else
        {
            GetComponent<Collider>().isTrigger = false; 
        }
     
    }

    void Dest()
    {
        Destroy(this.transform.parent.gameObject);
    }
}
