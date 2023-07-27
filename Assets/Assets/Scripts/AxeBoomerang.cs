using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBoomerang : MonoBehaviour
{
    bool go;
    GameObject player;
    private MeshRenderer swordImage;
    Transform itemToRotate;
    private BarbarWeaponCont weaponController1;
    Vector3 locationInFrontOfPlayer;  
    private Vector3 target2;
    private Transform target;

    void Start()
    {
        go = false;
       
        weaponController1=SharedVariables.Instance.weaponController;
        player = SharedVariables.Instance.playa;
        swordImage = SharedVariables.Instance.swordIm;
        swordImage.enabled = false;
        itemToRotate = gameObject.transform.GetChild(0);
        target = SharedVariables.Instance.axeAim;
        target2 = target.position;
        locationInFrontOfPlayer = target2;
        StartCoroutine(Boom());
    }
    
    IEnumerator Boom()
    {
        go = true;
        yield return new WaitForSeconds(.5f);
        go = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Untagged"))
        {
            go = false;
        }
    }
    void Update()
    {
        if (itemToRotate != null)
        {
            itemToRotate.transform.Rotate(Time.deltaTime * 2000, 0, 0); 
        }

        if (go)
        {
            transform.position = Vector3.MoveTowards(transform.position, locationInFrontOfPlayer, Time.deltaTime * 30); //Change The Position To The Location In Front Of The Player            
        }

        if (!go)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z), Time.deltaTime * 30); //Return To Player
        }

        if (!go && Vector3.Distance(player.transform.position, transform.position) < 1.5)
        {
            
            swordImage.enabled = true;
            Destroy(this.gameObject);
        }

    }
}
