using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeaponCont : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private AudioClip shotSound;
    private CharacterAnimation playerAnimation;
    public float shootInterval;
    public int Level;
    private void Start()
    {
        playerAnimation = GetComponentInParent<CharacterAnimation>();
        StartCoroutine(ShootContinuously());
       
    }
    private IEnumerator ShootContinuously()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }
    public void Shoot()
    {
        Instantiate(projectile, shootPosition.position, shootPosition.rotation);
        //playerAnimation.Attack(true);
        // plays the shot sound
        AudioController.instance.PlayOneShot(shotSound,1f);
    }
    public void Upgrade(int level)
    {
        Level += level;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
