
using UnityEngine;

public class Settlement : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Settlement"))
        {
            Destroy(this.gameObject);
        }
    }
}
