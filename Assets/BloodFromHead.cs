using System.Collections;

using UnityEngine;

public class BloodFromHead : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(2);
        this.gameObject.SetActive(false);
    }
    private void OnBecameInvisible()
    {
      this.gameObject.SetActive(false);
    }
}
