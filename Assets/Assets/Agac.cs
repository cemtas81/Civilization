
using UnityEngine;

public class Agac : MonoBehaviour
{
    private BarbScreenCont barb;
    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        barb = FindObjectOfType<BarbScreenCont>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            rb.isKinematic = false;
        }
        else if (other.CompareTag("Player") && rb.isKinematic != true)
        {
          
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player")&&rb.isKinematic!=true)
        {
            barb.UpdateWood(1);
            this.gameObject.SetActive(false);
        }
    }

}
