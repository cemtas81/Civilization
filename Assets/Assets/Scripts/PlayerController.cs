using cemtas81;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IKillable, ICurable {

	[HideInInspector]public Status playerStatus;
	[SerializeField] private LayerMask groundMask;
	[SerializeField] private ScreenController screenController;
	[SerializeField] private AudioClip damageSound;
	//public StarterAssetsInputs zone;
	private Vector3 direction;
	private PlayerMovement playerMovement;
	private CharacterAnimation playerAnimation;
	[SerializeField] private GameObject upgrade1,upgrade2,upgrade3,upgrade4,upgrade5,upgradePanel,sword,Bhole,arrowHolder;	
	[SerializeField] private int level_m;
    [SerializeField] private float activeDuration, inactiveDuration;
	private MainWeapon weapon;
	public WeaponController weaponController;
	private MyController myController1;
	private InputAction action1;
	public GamePadCursorController joyAim;
	public TargetMover cursorAim;
	private void Awake()
	{
		myController1=new MyController();
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimation = GetComponent<CharacterAnimation>();
        playerStatus = GetComponent<Status>();
		//zone = GetComponent<StarterAssetsInputs>();
		weapon = FindObjectOfType<MainWeapon>();
        weaponController = FindObjectOfType<WeaponController>();
    }
	private void OnEnable()
	{
		myController1.Enable();
		action1 = myController1.MyGameplay.MoveCursor;
	}
	private void OnDisable()
	{
		myController1.Disable();
	} 
    void Update () 
    {
        if (weaponController.gameObject.activeInHierarchy != true)
        {
            UnShoot();
        }
#if UNITY_STANDALONE
        //float xAxis = Input.GetAxisRaw("Horizontal");
		//float zAxis = Input.GetAxisRaw("Vertical");
        //joy.gameObject.SetActive(false);
		Gamepad gamepad=Gamepad.current;
		if (gamepad != null)
		{
			joyAim.enabled = true;
			cursorAim.enabled = false;
			Cursor.visible = false;	
		}
		else
		{
			joyAim.enabled=false;
			cursorAim.enabled=true;
			Cursor.visible=true;
	
		}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            screenController.Pause();
        }
		Vector2 moving=action1.ReadValue<Vector2>();
		float xAxis = moving.x;
		float zAxis = moving.y;
#endif
#if UNITY_ANDROID || UNITY_IPHONE
		
		float xAxis = zone.move.x;
		float zAxis = zone.move.y;
#endif

		// creates a Vector3 with the new direction
		direction = new Vector3 (xAxis, 0, zAxis);
		float velocityZ=Vector3.Dot(direction.normalized,transform.forward);
		float velocityX = Vector3.Dot(direction.normalized, transform.right);
		playerAnimation.VelocityZ(velocityZ);
		playerAnimation.VelocityX(velocityX);
		// player animations transition
		playerAnimation.Movement(direction.magnitude);
	}		
	void FixedUpdate () {
		// moves the player by second using physics
		// use physics (rigidbody) to compute the player movement is better than transform.position 
		// because prevents the player to "bug" when colliding with other objects
		playerMovement.Movement(direction, playerStatus.speed);
		if (arrowHolder.activeInHierarchy!=true)
		{
            playerMovement.PlayerRotation(groundMask);
        }
	
	}
	/// <summary>
	/// Loses health based on the damage value. 
	/// If health is equal to or less than 0 the game ends.
	/// </summary>
	/// <param name="damage">Damage taken.</param>
	public void LoseHealth (int damage) {
		playerStatus.health -= damage;
		screenController.UpdateHealthSlider();

		// plays the damage sound
		AudioController.instance.PlayOneShot(damageSound);
		
		if (playerStatus.health <= 0)
			Die();
	}
	/// <summary>
	/// Pauses the game and display the Game Over message on the screen.
	/// </summary>
	public void Die () 
	{
		screenController.GameOver();
		
	}
	public void Slashy()
	{
		weapon.Slash();
	}
	public void BoomerangShoot()
	{
      
        if (weaponController!=null)
		{
            weaponController.Shoot();
        }
	
	}
	public void UnShoot()
	{
		weaponController.enabled=false;
		sword.GetComponent<MeshRenderer>().enabled=false;
	}
    public void HealHealth(int amount) {
        playerStatus.health += amount;
		if (playerStatus.health > playerStatus.initialHealth)
			playerStatus.health = playerStatus.initialHealth;
		screenController.UpdateHealthSlider();
    }
	public void Spell(int weapon)
	{
       
        switch (weapon)
		{			
			case 0:
                upgrade1.SetActive(true);
                weaponController = FindObjectOfType<WeaponController>();
                weaponController.enabled = true;
                upgrade2.SetActive(false);
                upgrade3.SetActive(false);
                upgrade4.SetActive(false);
                upgrade5.SetActive(false);
                break ;
			case 1:
                upgrade1.SetActive(false);
                upgrade2.SetActive(true);
                upgrade3.SetActive(false);
                upgrade4.SetActive(false);
                upgrade5.SetActive(false);
                break;
            case 2:
                upgrade1.SetActive(false);
                upgrade2.SetActive(false);
                upgrade3.SetActive(true);
                upgrade4.SetActive(false);
                upgrade5.SetActive(false);
                break;
            case 3:
                upgrade1.SetActive(false);
                upgrade2.SetActive(false);
                upgrade3.SetActive(false);
                upgrade4.SetActive(true);
                upgrade5.SetActive(false);
                break;
            case 4:
                upgrade1.SetActive(false);
                upgrade2.SetActive(false);
                upgrade3.SetActive(false);
                upgrade4.SetActive(false);
                upgrade5.SetActive(true);
                break;
            default :
                upgrade1.SetActive(false);
                upgrade2.SetActive(false);
                upgrade3.SetActive(false);
                upgrade4.SetActive(false);
                upgrade5.SetActive(false);
                break;
				
        }
	}
}
