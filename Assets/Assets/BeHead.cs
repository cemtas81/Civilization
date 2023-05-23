
using UnityEngine;
public class BeHead : MonoBehaviour
{
    private Rigidbody body;
    private BarbScreenCont slider;
    public float expForce;
    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        slider = FindObjectOfType<BarbScreenCont>();
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
    void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
