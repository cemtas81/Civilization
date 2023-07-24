using UnityEngine;
using DamageNumbersPro;
using UnityEngine.AI;

public class BarbEnemyCont : MonoBehaviour, IKillable
{

    [HideInInspector] public EnemySpawner EnemySpawner;
    [SerializeField] private AudioClip deathSound, ThrowSound; 
    [SerializeField] private GameObject bloodParticle,aidKit, spear;
    private Status enemyStatus;
    private GameObject player;
    private CharacterMovement enemyMovement;
    private CharacterAnimation enemyAnimation;
    private BarbScreenCont screenController;
    public GameObject head, cust,randomClothes;
    private Vector3 direction;
    public float turnSpeed;
    private float probabilityAidKit = .08f;
    private MySolidSpawner Parent;
    public DamageNumber numberPrefab;
    public int random;
    private NavMeshAgent agent;
    //private SettlementSpawner settlement;
    public bool Ranged; 
    public Transform ThrowPos;
    private NavMeshObstacle obstacle;

    void Awake()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        player = SharedVariables.Instance.playa;
        enemyMovement = GetComponent<CharacterMovement>();
        enemyAnimation = GetComponent<CharacterAnimation>();
        enemyStatus = GetComponent<Status>();   
        screenController=SharedVariables.Instance.screenCont;     
        Parent=SharedVariables.Instance.spawner;
        GetRandomEnemy();
        Parent.spawnedPrefabs.Add(this.gameObject);
        enabled = true;
        agent = GetComponent<NavMeshAgent>();  
        enemyStatus.speed = Random.Range(2.6f, 3.1f);
        random = Random.Range(5, 10);
    }
  
    void FixedUpdate()
    {
        direction = player.transform.position - transform.position;
        direction.y = 0;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (Ranged != true)
        {
            if (direction != Vector3.zero && agent == null)
            {
                enemyAnimation.Movement(direction.magnitude * 5);
                enemyMovement.Rotation(direction);
            }

            if (distance > 60 && agent == null )
            {
                Parent.spawnedPrefabs.Remove(this.gameObject);
                Destroy(gameObject);
                //this.gameObject.SetActive(false);
                enabled = false;
                return;
            }
            else if (distance >= 2.1f)
            {

                if (agent != null)
                {
                    if (!agent.enabled)
                    {
                        obstacle.enabled = false;
                        agent.enabled = true;

                    }
                    else
                    {
                       
                        if (IsPlayerOnNavMesh())
                        {
                            enemyAnimation.Movement(direction.magnitude);
                            direction = player.transform.position;
                            enemyMovement.Movement(direction);
                            enemyAnimation.Attack(false);
                            Vector3 direction2 = direction - transform.position;
                            enemyMovement.Rotation(direction2);                          
                        }
                        else
                        {
                            // Stop the agent when the player is outside the navmesh                         
                            enemyAnimation.Movement(direction.magnitude);
                            direction = SharedVariables.Instance.gatherPoint.transform.position;
                            enemyMovement.Movement(direction);
                            enemyAnimation.Attack(false);
                            Vector3 direction2 = direction - transform.position;
                            enemyMovement.Rotation(direction2);
                        }
                    }

                }
                else
                {
                    enemyMovement.Movement(direction, enemyStatus.speed);
                    enemyAnimation.Attack(false);
                }

            }
            else
            {
                if (agent != null)
                {
                    if (agent.enabled)
                    {
                        agent.velocity = Vector3.zero;
                        agent.enabled = false;
                        obstacle.enabled = true;
                    }         
                    enemyMovement.Rotation(direction);
                    enemyAnimation.Attack(true);
                }
                else
                {                
                    direction = player.transform.position - transform.position;
                    enemyAnimation.Attack(true);
                }

            }
        }
        //      else if (distance > 30) 
        //{
        //	Rolling();
        //}
        else
        {
           
            enemyMovement.Rotation(direction);
            if (distance <= 10)
            {
               
                enemyAnimation.Attack2(true);
            }
            else
            {
                enemyAnimation.Attack2(false);
            }
        }

    }
    private bool IsPlayerOnNavMesh()
    {
        return NavMesh.SamplePosition(player.transform.position, out _, 1f, NavMesh.AllAreas);
    }
    void AttackPlayer()
    {       
        player.GetComponent<BarbCont2>().LoseHealth(random);
    } 
    void AttackPlayer2()
    {    
        Instantiate(spear, ThrowPos.position,ThrowPos.rotation);
        AudioController.instance.PlayOneShot(ThrowSound, 0.8f);
    }

    void GetRandomEnemy()
    {
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

