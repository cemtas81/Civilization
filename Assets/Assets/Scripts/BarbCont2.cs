using Pathfinding;
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
    [SerializeField] private float range; 
    [SerializeField] private GameObject bloodParticle, spearCase, spearHold;
    [SerializeField] private Transform bloodEffect;
    //private BarbarWeaponCont BarbarWeaponCont;
    //public StarterAssetsInputs joy;
    //public float nextUpdate;
    private bool canShoot, canThrow,dead;  
    MyController myController; 
    public GamePadCursorController joyAim;
    public TargetMover cursorAim;
    private InputAction action;
    private void Awake()
    {   
        myController = new MyController();
        myController.MyGameplay.WeaponSelect.performed += wpn => Atto();
        myController.MyGameplay.SecondWeapon.performed += wpn => Throw();         
        canShoot = true;
        canThrow = true;      
        screenController = FindObjectOfType<BarbScreenCont>();
        dead = false; 
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponentInChildren<CharacterAnimation>();
        playerStatus = GetComponent<Status>();      
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
        //float xAxis = Input.GetAxisRaw("Horizontal");
        //float zAxis = Input.GetAxisRaw("Vertical");
        Vector2 moving=action.ReadValue<Vector2>();
        float xAxis =moving.x;
        float zAxis = moving.y;
        //joy.gameObject.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            screenController.Pause();
        }
      
#endif
#if UNITY_ANDROID || UNITY_IPHONE
		
		float xAxis = zone.move.x;
		float zAxis = zone.move.y;
#endif

        // creates a Vector3 with the new direction
        direction = new Vector3(xAxis, 0, zAxis);
        float velocityZ = Vector3.Dot(direction.normalized, transform.forward);
        float velocityX = Vector3.Dot(direction.normalized, transform.right);
        playerAnimation.VelocityZ(velocityZ);
        playerAnimation.VelocityX(velocityX);
        // player animations transition
        playerAnimation.Movement(direction.magnitude);
    }

    void FixedUpdate()
    {
        // moves the player by second using physics
        // use physics (rigidbody) to compute the player movement is better than transform.position 
        // because prevents the player to "bug" when colliding with other objects
        playerMovement.Movement(direction, playerStatus.speed);    
        playerMovement.PlayerRotation(groundMask);

    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.5f);
        canShoot = true;
    }
    void Throw()
    {
        if (canThrow && canShoot)
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
        yield return new WaitForSeconds(0.2f);
        spearHold.SetActive(false);
        //BarbarWeaponCont.Shoot2();
        yield return new WaitForSeconds(0.8f);
        canThrow = true;
        spearCase.SetActive(true);
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
        //animator.SetBool("Block",true);
        playerAnimation.PlayerThrow();
    }
    /// <summary>
    /// Loses health based on the damage value. 
    /// If health is equal to or less than 0 the game ends.
    /// </summary>
    /// <param name="damage">Damage taken.</param>
    public void LoseHealth(int damage)
    {
        playerStatus.health -= damage;
        screenController.UpdateHealthSlider();
        Instantiate(bloodParticle, bloodEffect.transform.position, transform.rotation);
        // plays the damage sound
        AudioController.instance.PlayOneShot(damageSound,Random.Range(0.2f,0.9f));

        if (playerStatus.health <= 0)
            Die();
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
