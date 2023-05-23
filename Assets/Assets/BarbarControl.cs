using StarterAssets;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class BarbarControl : MonoBehaviour,IKillable,ICurable
{
    [HideInInspector] public Status playerStatus;
    //[SerializeField] private float range;
    //public Vector3 target;
    private Animator animator;
    [SerializeField] private AudioClip damageSound;
    [SerializeField] private GameObject bloodParticle,spearCase,spearHold;
    [SerializeField] private Transform bloodEffect;
    //private BarbarWeaponCont BarbarWeaponCont;
    public StarterAssetsInputs joy;
    //public float nextUpdate;
    private bool canShoot,canThrow;
    private Quaternion qTo;
    public float speed = 2.0f;
    MyController myController;
    private BarbScreenCont screenController;
    private bool dead;
    private Camera m_camera;
    public Vector3 target;
    private Animator ani;
    private Vector3 direction;
    private void Awake()
    {
        ani = GetComponent<Animator>();
        myController = new MyController();
        myController.MyGameplay.WeaponSelect.performed += wpn =>Atto() ;
        myController.MyGameplay.SecondWeapon.performed +=wpn =>Throw() ; 
        animator = GetComponent<Animator>();
        playerStatus = GetComponent<Status>();
        //BarbarWeaponCont = GetComponent<BarbarWeaponCont>();
        //Attacking();
        canShoot = true;
        canThrow = true;   
        qTo = transform.rotation;
        screenController=FindObjectOfType<BarbScreenCont>();
        dead = false;
        m_camera = Camera.main;
    }   
    void Update()
    {
#if UNITY_STANDALONE
        Gamepad gamepad = Gamepad.current;
        if (Input.GetMouseButtonDown(0) )
        {
            Atto();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Throw();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Again();
        }
       
#endif
#if UNITY_ANDROID || UNITY_IPHONE
        //if (Time.time >= nextUpdate)
        //{

        //    nextUpdate = Mathf.FloorToInt(Time.time) + 0.5f;

        //    UpdateTarget2();
        //}
        
#endif
    }
   void Again()
    {
        if (dead)
        {
            screenController.Restart();
        }
    }
    void Atto()
    {
        if (canShoot&&canThrow)
        {
            StartCoroutine(Attack());
            Attacking();
            canShoot = false;
        }
    }
    IEnumerator Attack()
    {

        //yield return new WaitForSeconds(.45f);
        //BarbarWeaponCont.Shoot();
        yield return new WaitForSeconds(1.5f);
        canShoot = true;
    }
    void Throw()
    {
        if (canThrow&&canShoot)
        {
            StartCoroutine(Throwy());
            Throwing();
            canThrow = false;
        }
    }
    IEnumerator Throwy()
    {
        yield return new WaitForSeconds(.5f);
        spearCase.SetActive(false);
        spearHold.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        spearHold.SetActive(false);
        //BarbarWeaponCont.Shoot2();
        yield return new WaitForSeconds(0.7f);
        canThrow = true;
        spearCase.SetActive(true);
    }
    public void Attacking()
    {

        animator.SetTrigger("Attack");
        //Quaternion newRotation = Quaternion.LookRotation(direction);

        //transform.rotation = newRotation;
    }
    public void Throwing()
    {
        //animator.SetBool("Block",true);
        animator.SetTrigger("Throw");
    }
    public void HealHealth(int amount)
    {
        playerStatus.health += amount;
        if (playerStatus.health > playerStatus.initialHealth)
            playerStatus.health = playerStatus.initialHealth;
        screenController.UpdateHealthSlider();
    }

    public void LoseHealth(int damage)
    {
        playerStatus.health -= damage;
        screenController.UpdateHealthSlider();
        Instantiate(bloodParticle,bloodEffect.transform.position,transform.rotation);
        // plays the damage sound
        AudioController.instance.PlayOneShot(damageSound);
        if (playerStatus.health <= 0)
            Die();
    }
    public void Die()
    {
        screenController.GameOver();
        dead = true;
    }
    private void OnEnable()
    {
        myController.Enable();

    }
    private void OnDisable()
    {
        myController.Disable();
    }
}
