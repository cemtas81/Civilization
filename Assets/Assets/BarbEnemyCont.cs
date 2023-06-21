using UnityEngine;
using DamageNumbersPro;
using UnityEngine.AI;

public class BarbEnemyCont : MonoBehaviour, IKillable
{

    [HideInInspector] public EnemySpawner EnemySpawner;

    [SerializeField] private AudioClip deathSound;
    [SerializeField] private GameObject aidKit;
    [SerializeField] private GameObject bloodParticle;

    private Status enemyStatus;
    private GameObject player;
    private CharacterMovement enemyMovement;
    private CharacterAnimation enemyAnimation;
    private BarbScreenCont screenController;
    public GameObject head, cust,randomClothes;
    private Vector3 direction;
    //public Rigidbody[] rigids;
    private float probabilityAidKit = .08f;
    private MySolidSpawner Parent;
    public DamageNumber numberPrefab;
    public int random;
    private NavMeshAgent agent;
    private SettlementSpawner settlement;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyMovement = GetComponent<CharacterMovement>();
        enemyAnimation = GetComponent<CharacterAnimation>();
        enemyStatus = GetComponent<Status>();
        screenController = FindObjectOfType<BarbScreenCont>();
        Parent = FindObjectOfType<MySolidSpawner>();
        GetRandomEnemy();
        Parent.spawnedPrefabs.Add(this.gameObject);
        enabled = true;
        agent = GetComponent<NavMeshAgent>();
        settlement=FindObjectOfType<SettlementSpawner>();
        if (agent!=null)
        {
            settlement.soldiers++;
        }
    }
  
    void FixedUpdate()
    {

        // get the distance between this enemy and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (direction != Vector3.zero && agent == null)
        {
            enemyAnimation.Movement(direction.magnitude * 5);
            enemyMovement.Rotation(direction);
        }
        if (distance>=40&&agent!=null)
        {
            enemyAnimation.Movement(0);
        }
        if (distance<40&&agent!=null)
        {
            enemyAnimation.Movement(direction.magnitude * 5);
        }
        if (distance > 60 && agent == null)
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
        else if (distance >= 2.1f)
        {
         
            direction = player.transform.position - transform.position;
            direction.y = 0;
      
           
            if (agent != null)
            {

                direction = player.transform.position;
                enemyMovement.Movement(direction);
            }
            else
                enemyMovement.Movement(direction, enemyStatus.speed);
                enemyAnimation.Attack(false);

        }
        else
        {
            if (agent != null)
            {
  
                direction = player.transform.position;
                enemyAnimation.Attack(true);
            }
            else
                direction = player.transform.position - transform.position;
                // otherwise, the Attacking animation is on
                enemyAnimation.Attack(true);
        }

    }

    /// <summary>
    /// Attacks the player, causing a random damage between 20 and 30.
    /// </summary>
    void AttackPlayer()
    {
        int damage = Random.Range(5, 10);
        player.GetComponent<BarbCont2>().LoseHealth(damage);
    }

    void GetRandomEnemy()
    {
        // gets a random enemy
        // (the Zombie prefab has 27 different zombie models inside it)
        int randomEnemy = Random.Range(1, randomClothes.transform.childCount);
        randomClothes.transform.GetChild(randomEnemy).gameObject.SetActive(true);
    }

    public void LoseHealth(int damage)
    {
        enemyStatus.health -= damage;

        if (enemyStatus.health <= 0)
        {
            //DamageN(.5f, "execution");
            Die();
        }
        //else
        //    DamageN(1.5f, "hit");
    }

    public void Die()
    {
        Destroy(gameObject, 1.5f);
        enemyAnimation.Die();
        enemyMovement.Die();
        //enemyState=EnemyState.Ragdoll;
        //RagDoll();
      
        if (agent!=null)
        {
            agent.enabled = false;
        }
        enabled = false;
        screenController.UpdateDeadZombiesCount();
        head.SetActive(false);
        cust.SetActive(false);
        Parent.Spawn3(this.transform.position);
        Parent.spawnedPrefabs.Remove(this.gameObject);
        // plays the death sound
        AudioController.instance.PlayOneShot(deathSound,Random.Range(0.2f, 0.9f));
        InstantiateAidKit(probabilityAidKit);
    }

    public void BloodParticle(Vector3 position, Quaternion rotation)
    {
        Instantiate(bloodParticle, position, rotation);
    }

    private void InstantiateAidKit(float probability)
    {
        if (Random.value <= probability)
            Instantiate(aidKit, transform.position, Quaternion.identity);
    }

    void DamageN(float value, string st)
    {
        DamageNumber damageNumber = numberPrefab.Spawn(transform.position + Vector3.up * value, st);
    }
}

