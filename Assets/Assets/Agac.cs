
using UnityEngine;

public class Agac : MonoBehaviour
{
    private BarbScreenCont barb;
    private Rigidbody rb;
    public GameObject effect;
    //private MeshCollider coll;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        barb = FindObjectOfType<BarbScreenCont>();
        //coll = GetComponent<MeshCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Sword"))
        {
            rb.isKinematic = false;
            //coll.convex = false;    
            rb.AddForceAtPosition(100 * Time.deltaTime * Vector3.forward, Vector3.forward,ForceMode.Impulse); 
         
        }
        else if (other.CompareTag("Settlement") && rb.isKinematic == true)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Quaternion rotation = Quaternion.LookRotation(-transform.forward);
        if (collision.collider.CompareTag("Player")&&rb.isKinematic!=true)
        {
            barb.UpdateWood(1);
            this.gameObject.SetActive(false);
            Instantiate(effect,transform.position, rotation);
        }
    
    }

}
