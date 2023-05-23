
using UnityEngine;

public class BarbarWeaponCont : MonoBehaviour
{
    [SerializeField] private GameObject projectile,spear,Sword;
    [SerializeField] private Transform shootPosition,pos2;
    [SerializeField] private AudioClip shotSound,sound2;
    
    //public void ShootAxe()
    //{

    //    GameObject prefabToSpawn = AxePool.SharedInstance.GetPooledObject();
    //    if (prefabToSpawn == null) return;
    //    prefabToSpawn.transform.SetPositionAndRotation(shootPosition.position, shootPosition.rotation);
    //    // Enable the prefab
    //    prefabToSpawn.SetActive(true);
    //    AudioController.instance.PlayOneShot(shotSound, 0.5f);
    //}
    //public void ShootSpear()
    //{

    //    GameObject prefabToSpawn = SpearPool.SharedInstance.GetPooledObject();
    //    if (prefabToSpawn == null) return;
    //    prefabToSpawn.transform.SetPositionAndRotation(pos2.position,pos2.rotation);
    //    // Enable the prefab
    //    prefabToSpawn.SetActive(true);
    //    AudioController.instance.PlayOneShot(sound2, 0.6f);
    //}
    public void Shoot()
    {
       
        Instantiate(projectile, shootPosition.position, shootPosition.rotation);

        // plays the shot sound
        AudioController.instance.PlayOneShot(shotSound, 0.5f);
    } 
    public void Shoot2()
    {

        Instantiate(spear, pos2.position, pos2.rotation);      
        // plays the shot sound
        AudioController.instance.PlayOneShot(sound2, 0.6f);
    }
   
}
