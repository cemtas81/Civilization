using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneDissolve : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Dissolve());
    } 
    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(3);
        this.gameObject.SetActive(false);
    }
    void OnBecameInvisible()
    {
        this.gameObject.SetActive(false);
    }
}
