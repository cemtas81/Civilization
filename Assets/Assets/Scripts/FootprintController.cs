using System.Collections;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    private ObjectPooling2 objectPooling; 
    public Transform leftFootPos;
    public Transform rightFootPos;
    public float footprintOffset = 0.05f; // Offset value to raise the footprint above the hit point

    private void Start()
    {
        objectPooling = ObjectPooling2.SharedInstance; 
    }

    void LeftPrint()
    {
        RaycastHit hit;
        if (Physics.Raycast(leftFootPos.position, leftFootPos.forward, out hit,0.2f))
        {
            GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[0]); 

            if (footprintPrefab != null)
            {
                Vector3 footprintPosition = hit.point + hit.normal * footprintOffset; // Calculate the footprint position with an offset
                footprintPrefab.transform.SetPositionAndRotation(footprintPosition, Quaternion.LookRotation(hit.normal, leftFootPos.up));
                footprintPrefab.SetActive(true);

                StartCoroutine(DeactivateFootstepDelayed(footprintPrefab));
            }
        }
    }

    void RightPrint()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightFootPos.position, rightFootPos.forward, out hit,0.2f))
        {
            GameObject footprintPrefab = objectPooling.GetPooledObject(objectPooling.objectsToPool[1]); 
            if (footprintPrefab != null)
            {
                Vector3 footprintPosition = hit.point + hit.normal * footprintOffset; // Calculate the footprint position with an offset
                footprintPrefab.transform.SetPositionAndRotation(footprintPosition, Quaternion.LookRotation(hit.normal, rightFootPos.up));
                footprintPrefab.SetActive(true);

                StartCoroutine(DeactivateFootstepDelayed(footprintPrefab));
            }
        }
    }

    private IEnumerator DeactivateFootstepDelayed(GameObject footprintPrefab)
    {
        yield return new WaitForSeconds(1f); 
        footprintPrefab.SetActive(false);
    }
}
