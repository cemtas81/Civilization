using UnityEngine;
using DamageNumbersPro;

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
    private enum EnemyState 
    {
        Walking,
        Ragdoll
    }
    private EnemyState enemyState=EnemyState.Walking;
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
        //rigids = GetComponentsInChildren<Rigidbody>();
        //Walk();
    }
    //private void Update()
    //{
    //    switch (enemyState)
    //    {
    //        case EnemyState.Walking:
    //            Walk();
    //            break;
    //       case EnemyState.Ragdoll:
    //            RagDoll();
    //            break;
    //    }
    //}
    //void Walk()
    //{
    //    foreach (var rig in rigids)
    //    {
    //        rig.isKinematic = true;
    //    }
    //}
    //void RagDoll()
    //{
    //    foreach (var rig in rigids)
    //    {
    //        rig.isKinematic = false;
    //    }
    //}
    void FixedUpdate()
    {

        // get the distance between this enemy and the player
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (direction != Vector3.zero)
        {
            enemyMovement.Rotation(direction);
        }

        enemyAnimation.Movement(direction.magnitude * 5);
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
        else if (distance > 2.1f)
        {
            // get the final position, that is, 
            // the distance between the enemy and the player
            direction = player.transform.position - transform.position;
            direction.y = 0;
            // checks if enemy and player are not colliding.
            // The 2.5f is because both enemy and player have a Capsule Collider with radius equal 1,
            // so if the distance is bigger than both radius they are colliding
            enemyMovement.Movement(direction, enemyStatus.speed);

            // if they're not colliding the Attacking animation is off
            enemyAnimation.Attack(false);
        }
        else
        {
            direction = player.transform.position - transform.position;
            direction.y = 0;
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
            DamageN(.5f, "execution");
            Die();
        }
        else
            DamageN(1.5f, "hit");
    }

    public void Die()
    {
        Destroy(gameObject, 1.5f);
        enemyAnimation.Die();
        enemyMovement.Die();
        //enemyState=EnemyState.Ragdoll;
        //RagDoll();
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

