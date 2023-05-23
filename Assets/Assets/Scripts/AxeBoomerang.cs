using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeBoomerang : MonoBehaviour
{
    bool go;//Will Be Used To Change Direction Of Weapon

    GameObject player;//Reference To The Main Character
    GameObject sword;//Reference To The Main Character's Weapon
    private MeshRenderer swordImage;
    Transform itemToRotate;//The Weapon That Is A Child Of The Empty Game Object
    private WeaponController weaponController;
    Vector3 locationInFrontOfPlayer;//Location In Front Of Player To Travel To
  
    private Vector3 target2;
    private Transform target;
   
    // Use this for initialization
    void Start()
    {
        go = false; //Set To Not Return Yet
        weaponController = FindObjectOfType<WeaponController>();
        player = GameObject.FindGameObjectWithTag("Player");
        sword = GameObject.FindGameObjectWithTag("boomerang");
        swordImage = sword.GetComponent<MeshRenderer>();
        swordImage.enabled = false; //Turn Off The Mesh Render To Make The Weapon Invisible

        itemToRotate = gameObject.transform.GetChild(0); //Find The Weapon That Is The Child Of The Empty Object       

        //Adjust The Location Of The Player Accordingly, Here I Add To The Y position So That The Object Doesn't Go Too Low ...Also Pick A Location In Front Of The Player
        //locationInFrontOfPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z) + player.transform.forward * 10f;
        target = GameObject.FindWithTag("Aim").GetComponent<Transform>();
        target2 = target.position;
        locationInFrontOfPlayer = target2;
        StartCoroutine(Boom());//Now Start The Coroutine
    }
    
    IEnumerator Boom()
    {
        go = true;
        yield return new WaitForSeconds(1f);//Any Amount Of Time You Want
        go = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (itemToRotate != null)
        {
            itemToRotate.transform.Rotate(Time.deltaTime * 1500, 0, 0); //Rotate The Object
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
            //Once It Is Close To The Player, Make The Player's Normal Weapon Visible, and Destroy The Clone
            swordImage.enabled = true;
            Destroy(this.gameObject);
        }

    }
}
