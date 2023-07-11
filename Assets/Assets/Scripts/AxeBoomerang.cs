using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBoomerang : MonoBehaviour
{
    bool go;
    GameObject player;
    private MeshRenderer swordImage;
    Transform itemToRotate;
    private WeaponController weaponController;
    Vector3 locationInFrontOfPlayer;  
    private Vector3 target2;
    private Transform target;

    void Start()
    {
        go = false;
       
        weaponController=SharedVariables.weaponController;
        player = SharedVariables.playa;
        swordImage = SharedVariables.swordIm;
        swordImage.enabled = false;

        itemToRotate = gameObject.transform.GetChild(0);      

        target = GameObject.FindWithTag("Aim").GetComponent<Transform>();
        target2 = target.position;
        locationInFrontOfPlayer = target2;
        StartCoroutine(Boom());
    }
    
    IEnumerator Boom()
    {
        go = true;
        yield return new WaitForSeconds(1f);
        go = false;
    }

    void Update()
    {
        if (itemToRotate != null)
        {
            itemToRotate.transform.Rotate(Time.deltaTime * 1500, 0, 0); 
        }

        if (go)
        {
            transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 20); //Change The Position To The Location In Front Of The Player            
        }

        if (!go)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Time.deltaTime * 25); //Return To Player
        }

        if (!go && Vector3.Distance(player.transform.position, transform.position) < 1.5)
        {
            
            swordImage.enabled = true;
            Destroy(this.gameObject);
        }

    }
}
