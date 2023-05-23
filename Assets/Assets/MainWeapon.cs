
using System.Collections;
using UnityEngine;

public class MainWeapon :MonoBehaviour
{
    //public float interval = 1.0f;
    public GameObject weapon;
   
    //IEnumerator Start()
    //{
    //    while (true)
    //    {
    //        _ = Instantiate(weapon, transform.position,transform.rotation);
           
    //        yield return new WaitForSeconds(interval);
    //    }
    //}
    public void Slash()
    {
        _ = Instantiate(weapon, transform.position, transform.rotation);
    }
}
