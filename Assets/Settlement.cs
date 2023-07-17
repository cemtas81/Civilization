using UnityEngine.AI;
using UnityEngine;

public class Settlement : MonoBehaviour
{
    private void Awake()
    {
        NavMesh.avoidancePredictionTime = 0.1f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Settlement"))
        {
            Destroy(gameObject);
        }
    }
}
