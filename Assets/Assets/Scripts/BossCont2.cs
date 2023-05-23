using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossCont2 : MonoBehaviour, IKillable
{

    [SerializeField] private GameObject aidKitPrefab, upgradePrefab, bloodParticle;
    [SerializeField] private Slider bossSlider;
    [SerializeField] private Image sliderImage;
    [SerializeField] private Color maxHealthColor, minHealthColor;
    private Vector3 direction;
    private Transform player;
    private MySolidSpawner Parent;
    private Status bossStatus;
    private CharacterAnimation bossAnimation;
    private CharacterMovement bossMovement;
    private float probabilityupgrade = .5f;
    private ScreenController screenController;
   
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        screenController = GameObject.FindObjectOfType<ScreenController>();
        bossStatus = GetComponent<Status>();
        Parent = FindObjectOfType<MySolidSpawner>();
        bossAnimation = GetComponent<CharacterAnimation>();
        bossMovement = GetComponent<CharacterMovement>();
      
        bossSlider.maxValue = bossStatus.initialHealth;
        screenController.ShowBossText();
        UpdateInterface();
       
    }
  
    void FixedUpdate()
    {

        // get the distance between this enemy and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (direction!=Vector3.zero)
		{
            bossMovement.Rotation(direction);
        }
        bossAnimation.Movement(direction.magnitude * 5);
        if (distance > 60)
        {
            Parent.spawnedPrefabs.Remove(this.gameObject);
            Destroy(gameObject);
            //this.gameObject.SetActive(false);

            enabled = false;
        }
        //      else if (distance > 30) 
        //{
        //	Rolling();
        //} 
        else if (distance > 3.5f)
        {
            
            // the distance between the enemy and the player
            direction = player.transform.position - transform.position;
  
            bossMovement.Movement(direction, bossStatus.speed);

            // if they're not colliding the Attacking animation is off
            bossAnimation.Attack(false);
        }
        else
        {
            direction = player.transform.position - transform.position;
            // otherwise, the Attacking animation is on
            bossAnimation.Attack(true);
        }
    }

    private void AttackPlayer()
    {
        int damage = Random.Range(30, 40);
        player.GetComponent<PlayerController>().LoseHealth(damage);
    }

    public void LoseHealth(int damage)
    {
        bossStatus.health -= damage;
        UpdateInterface();
        if (bossStatus.health <= 0)
            Die();
 
    }

    public void Die()
    {
        Parent.BossHere = false;
        Destroy(gameObject, 2);
        bossAnimation.Die();
        bossMovement.Die();
        Instantiate(aidKitPrefab, transform.position, Quaternion.identity);
        enabled = false;
        InstantiateUpgrade(probabilityupgrade);
    }

    public void BloodParticle(Vector3 position, Quaternion rotation)
    {
        Instantiate(bloodParticle, position, rotation);
    }

    private void UpdateInterface()
    {
        bossSlider.value = bossStatus.health;
        float healthPercent = (float)bossStatus.health / bossStatus.initialHealth;
        Color healthColor = Color.Lerp(minHealthColor, maxHealthColor, healthPercent);
        sliderImage.color = healthColor;
    }
    private void InstantiateUpgrade(float probability)
    {
        if (Random.value <= probability)
            Instantiate(upgradePrefab, new Vector3(transform.position.x+2,0,transform.position.z), Quaternion.identity);
    }
}