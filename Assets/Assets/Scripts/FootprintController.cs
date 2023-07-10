using System.Collections;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    private ObjectPooling2 objectPooling; // Reference to the ObjectPooling2 script
    public Transform leftFootPos;
    public Transform rightFootPos;
    public float footprintOffset = 0.05f; // Offset value to raise the footprint above the hit point

    private void Start()
    {
        objectPooling = ObjectPooling2.SharedInstance; // Get a reference to the ObjectPooling2 instance
    }

    void LeftPrint()
    {
        RaycastHit hit;
        if (Physics.Raycast(leftFootPos.position, leftFootPos.forward, out hit))
        {
            GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[0]); // Get a pooled footstep prefab

            if (footprintPrefab != null)
            {
                Vector3 footprintPosition = hit.point + hit.normal * footprintOffset; // Calculate the footprint position with an offset
                footprintPrefab.transform.SetPositionAndRotation(footprintPosition, Quaternion.LookRotation(hit.normal, leftFootPos.up));
                footprintPrefab.SetActive(true);

                // Deactivate the footstep prefab after a delay
                StartCoroutine(DeactivateFootstepDelayed(footprintPrefab));
            }
        }
    }

    void RightPrint()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightFootPos.position, rightFootPos.forward, out hit))
        {
            GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[1]); // Get a pooled footstep prefab

            if (footprintPrefab != null)
            {
                Vector3 footprintPosition = hit.point + hit.normal * footprintOffset; // Calculate the footprint position with an offset
                footprintPrefab.transform.SetPositionAndRotation(footprintPosition, Quaternion.LookRotation(hit.normal, rightFootPos.up));
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
