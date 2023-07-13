using Pathfinding.Examples;
using UnityEngine;

public class SharedVariables : MonoBehaviour
{
    private static SharedVariables instance;
    public static SharedVariables Instance { get { return instance; } }

    public PlayerMovement plyrmvmnt;
    public GameObject playa, sword, Ammo;
    public BarbCont2 cont;
    public MeshRenderer swordIm;
    public BarbScreenCont screenCont;
    public MySolidSpawner spawner;
    public SettlementSpawner settlementSpawner;
    public BarbarWeaponCont weaponController;
    public AudioSource audioS;
    public AstarSmoothFollow2 astar;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);

        plyrmvmnt = FindObjectOfType<PlayerMovement>();
        cont = FindObjectOfType<BarbCont2>();
        sword = GameObject.FindGameObjectWithTag("boomerang");
        playa = cont.gameObject;
        swordIm = sword.GetComponent<MeshRenderer>();
        screenCont = FindObjectOfType<BarbScreenCont>();
        spawner = FindObjectOfType<MySolidSpawner>();
        settlementSpawner = FindObjectOfType<SettlementSpawner>();
        weaponController = FindObjectOfType<BarbarWeaponCont>();
        audioS = FindObjectOfType<AudioSource>();
        Ammo = GameObject.FindGameObjectWithTag("Spear");
        astar = FindObjectOfType<AstarSmoothFollow2>();
    }
}
