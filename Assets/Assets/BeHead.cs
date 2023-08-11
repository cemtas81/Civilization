

using UnityEngine;
public class BeHead : MonoBehaviour
{
    private Rigidbody body;
    private BarbScreenCont slider;
    public float expForce, bloodprintOffset;
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
        Invoke(nameof(Dissolve), 3);
    }
    void Dissolve()
    {
        this.gameObject.SetActive(false);
    }
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
                GameObject bloodprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[2]);

                if (bloodprintPrefab != null)
                {

                    Vector3 bloodprintPosition = hit.point + hit.normal * bloodprintOffset; // Calculate the footprint position with an offset
                    bloodprintPrefab.transform.SetPositionAndRotation(bloodprintPosition, Quaternion.LookRotation(hit.normal, Pos.up));
                    bloodprintPrefab.SetActive(true);

                }

            }

        }
   
    }
  
   //void OnBecameInvisible()
   // {
   //     this.gameObject.SetActive(false);
   // }
}
