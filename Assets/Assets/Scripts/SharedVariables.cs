using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharedVariables : MonoBehaviour
{
    public static PlayerMovement plyrmvmnt;
    public static GameObject playa, sword;
    public static BarbCont2 cont;
    public static MeshRenderer swordIm;
    public static BarbScreenCont screenCont;
    public static MySolidSpawner spawner;
    public static SettlementSpawner settlementSpawner;
    public static WeaponController weaponController;
    public static AudioSource audioS;
    private void Awake()
    {
        plyrmvmnt=FindObjectOfType<PlayerMovement>();   
        cont=FindObjectOfType<BarbCont2>();
        sword=GameObject.FindGameObjectWithTag("boomerang");
        playa =cont.gameObject;
        swordIm=sword.GetComponent<MeshRenderer>();
        screenCont=FindObjectOfType<BarbScreenCont>();
        spawner=FindObjectOfType<MySolidSpawner>();
        settlementSpawner=FindObjectOfType<SettlementSpawner>(); 
        weaponController=FindObjectOfType<WeaponController>();
        audioS=FindObjectOfType<AudioSource>();
    }
}
