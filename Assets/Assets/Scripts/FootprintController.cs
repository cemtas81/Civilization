using System.Collections;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    private ObjectPooling2 objectPooling; // Reference to the ObjectPooling2 script

    private void Start()
    {
        objectPooling = ObjectPooling2.SharedInstance; // Get a reference to the ObjectPooling2 instance
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            Vector3 position = other.ClosestPoint(transform.position);
            Quaternion rotation = Quaternion.LookRotation(other.transform.up, transform.up);
            position += Vector3.up * 0.01f;
            GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[0]); // Get a pooled footstep prefab

            if (footprintPrefab != null)
            {
                footprintPrefab.transform.SetPositionAndRotation(position, rotation);
                footprintPrefab.SetActive(true);

                // Deactivate the footstep prefab after a delay
                StartCoroutine(DeactivateFootstepDelayed(footprintPrefab));
            }
        }
    }

    private IEnumerator DeactivateFootstepDelayed(GameObject footprintPrefab)
    {
        yield return new WaitForSeconds(1f); // Change the delay as per your requirement
        footprintPrefab.SetActive(false);
    }
}
