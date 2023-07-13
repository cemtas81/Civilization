
using System.Collections;
using UnityEngine;
public class BeHead : MonoBehaviour
{
    private Rigidbody body;
    private BarbScreenCont slider;
    public float expForce, footprintOffset;
    private ObjectPooling2 objectPooling;
    public Transform Pos;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        slider=SharedVariables.Instance.screenCont;
        objectPooling = ObjectPooling2.SharedInstance;
    }
    private void OnEnable()
    {
        body.AddRelativeForce(expForce * Time.deltaTime * Vector3.right,ForceMode.Impulse);
        //Invoke("Dissolve",2);
    }
    //void Dissolve()
    //{
    //    this.gameObject.SetActive(false);
    //} 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            this.gameObject.SetActive(false);
            slider.UpdateHead(1);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Finish")||collision.collider.CompareTag("Settlement"))
        {
            RaycastHit hit;
            if (Physics.Raycast(Pos.position, Pos.forward, out hit, 0.1f))
            {
                GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[2]);

                if (footprintPrefab != null)
                {

                    Vector3 footprintPosition = hit.point + hit.normal * footprintOffset; // Calculate the footprint position with an offset
                    footprintPrefab.transform.SetPositionAndRotation(footprintPosition, Quaternion.LookRotation(hit.normal, Pos.up));
                    footprintPrefab.SetActive(true);

                }

            }

        }

    }
  
   void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
