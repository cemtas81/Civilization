using System.Collections;
using UnityEngine;

public class FootprintController : MonoBehaviour
{
    private ObjectPooling2 objectPooling; 
    public Transform leftFootPos;
    public Transform rightFootPos;
    public float footprintOffset = 0.05f; // Offset value to raise the footprint above the hit point
    private AudioSource audio1;
    public AudioClip[] FootstepAudioClips;
    private void Start()
    {
        audio1 = SharedVariables.Instance.audioS;
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
     private void Step(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
           
            if (FootstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, FootstepAudioClips.Length);
                audio1.PlayOneShot(FootstepAudioClips[index], Random.Range(0.25f, 0.5f));
            }
        }
            
    }
    private IEnumerator DeactivateFootstepDelayed(GameObject footprintPrefab)
    {
        yield return new WaitForSeconds(1f); 
        footprintPrefab.SetActive(false);
    }
   
}
