using Pathfinding;
using Pathfinding.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BarbCont2 : MonoBehaviour, IKillable, ICurable
{
    [HideInInspector] public Status playerStatus;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private BarbScreenCont screenController;
    [SerializeField] private AudioClip damageSound;
    private Vector3 direction;
    private PlayerMovement playerMovement;
    private CharacterAnimation playerAnimation;
    [SerializeField] private GameObject bloodParticle, spearCase, spearHold,ammo;
    [SerializeField] private Transform bloodEffect;
    private bool canShoot, canThrow,dead;  
    MyController myController; 
    public GamePadCursorController joyAim;
    public TargetMover cursorAim;
    private InputAction action;
    private AstarSmoothFollow2 map;
    private AudioSource audio1;
   
    private void Awake()
    {   
        myController = new MyController();
        myController.MyGameplay.WeaponSelect.performed += wpn => Atto();
        myController.MyGameplay.SecondWeapon.performed += wpn => Throw();
        myController.MyGameplay.MakeAmmo.performed += wpn => MakeSpear();
        canShoot = true;
        canThrow = true;    
        screenController =FindObjectOfType<BarbScreenCont>();
        dead = false; 
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponentInChildren<CharacterAnimation>();
        playerStatus = GetComponent<Status>();      
        map = FindObjectOfType<AstarSmoothFollow2>();
        audio1 = FindObjectOfType<AudioSource>();
        ammo = GameObject.FindGameObjectWithTag("Spear"); 
    }
    private void OnEnable()
    {
        myController.Enable();
        action = myController.MyGameplay.MoveCursor;
    }
   
    void Update()
    {

#if UNITY_STANDALONE
        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            joyAim.enabled = true;
            cursorAim.enabled = false;
            Cursor.visible = false;
        }
        else
        {
            joyAim.enabled = false;
            cursorAim.enabled = true;
            Cursor.visible = true;
        }
        if (Input.GetMouseButtonDown(0))
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
        if (Input.GetKeyDown(KeyCode.X))
        {
            Special1();
        }
        Vector2 moving=action.ReadValue<Vector2>();
        float xAxis =moving.x;
        float zAxis = moving.y;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            screenController.Pause();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            map.enabled=!map.enabled;
            map.GetComponent<Camera>().enabled = !map.GetComponent<Camera>().enabled;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MakeSpear();
        }
        if (screenController.canSpecial==false)
        {
            playerAnimation.Special1(false);
        }
      
#endif
#if UNITY_ANDROID || UNITY_IPHONE
		
		float xAxis = zone.move.x;
		float zAxis = zone.move.y;
#endif
        direction = new Vector3(xAxis, 0, zAxis);
        float velocityZ = Vector3.Dot(direction.normalized, transform.forward);
        float velocityX = Vector3.Dot(direction.normalized, transform.right);
        playerAnimation.VelocityZ(velocityZ);
        playerAnimation.VelocityX(velocityX);
        playerAnimation.Movement(direction.magnitude);
    }
    void FixedUpdate()
    {   
        playerMovement.Movement(direction, playerStatus.speed);    
        playerMovement.PlayerRotation(groundMask);
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.2f);
        canShoot = true;
    }
    void Throw()
    {
        if (canThrow && canShoot&&screenController.spearCount>=1)
        {
            StartCoroutine(Throwy());
            Throwing();
            canThrow = false;
            screenController.DecreaseSpear(1);
            if (screenController.spearCount < 1)
                StartCoroutine(Thrown());
        }        
    }
    IEnumerator Thrown()
    {
        yield return new WaitForSeconds(.5f);
        ammo.GetComponent<MeshRenderer>().enabled=false;
    }
    IEnumerator Throwy()
    {
        yield return new WaitForSeconds(1.5f);
        canThrow = true;   
    }
    void Special1()
    {      
        playerAnimation.Special1(screenController.canSpecial);   
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
        if (canShoot && canThrow)
        {
            StartCoroutine(Attack());
            Attacking();
            canShoot = false;
        }
    }
    public void Attacking()
    {
        playerAnimation.PlayerAttack();    
    }
    public void Throwing()
    {
        playerAnimation.PlayerThrow();
    }

    public void LoseHealth(int damage)
    {
        playerStatus.health -= damage;
        screenController.UpdateHealthSlider();
        Instantiate(bloodParticle, bloodEffect.transform.position, transform.rotation);
        audio1.PlayOneShot(damageSound,Random.Range(0.2f,0.9f));
        if (playerStatus.health <= 0)
            Die();
    }
    public void MakeSpear()
    {
        screenController.UpdateSpear(1);
    }
    public void Die()
    {
        screenController.GameOver();
        dead = true;
    }  
    public void HealHealth(int amount)
    {
        playerStatus.health += amount;
        if (playerStatus.health > playerStatus.initialHealth)
            playerStatus.health = playerStatus.initialHealth;
        screenController.UpdateHealthSlider();
    }   
    private void OnDisable()
    {
        myController.Disable();
    }
}
