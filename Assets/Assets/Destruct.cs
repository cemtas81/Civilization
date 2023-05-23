using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruct : MonoBehaviour
{
    public float timer,collTimer;
    private Collider coll;
    void Start()
    {
        Destroy(gameObject,timer);
        StartCoroutine(KillZone());
        coll = GetComponent<Collider>();
    }
    IEnumerator KillZone()
    {
        yield return new WaitForSeconds(collTimer);
        coll.enabled = false;
    }
}
