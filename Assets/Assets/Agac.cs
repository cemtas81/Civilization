
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
            rb.AddForceAtPosition(100 * Time.deltaTime * Vector3.forward, Vector3.forward,ForceMode.Impulse); 
         
        }
        else if (other.CompareTag("Settlement") && rb.isKinematic == true)
        {
            Destroy(this.gameObject);
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
