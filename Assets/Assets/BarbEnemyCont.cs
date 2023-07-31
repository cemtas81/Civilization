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
    public bool Ranged, isBoss; 
    public Transform ThrowPos;
    private NavMeshObstacle obstacle;
    private bool playerInSight;
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
        if (isBoss)
        {
            random = 20;
        }
        else
        {
            random = Random.Range(5, 10);
        }
    }
  
    void FixedUpdate()
    {
        direction = player.transform.position - transform.position;
        direction.y = 0;
        float distance = Vector3.Distance(transform.position, player.transform.position);      
        if (!Ranged)
        {
            if (direction != Vector3.zero && agent == null)
            {
                enemyAnimation.Movement(direction.magnitude);
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
                            direction = player.transform.position;                                      
                        }
                        else
                        {                   
                            direction = SharedVariables.Instance.gatherPoint.transform.position;                    
                        }
                        enemyMovement.Rotation(direction);
                        enemyMovement.Movement(direction);
                        enemyAnimation.Movement(direction.magnitude);
                    }
                    enemyAnimation.Attack(false);
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

        else
        {

            Vector3 rayOrigin = transform.position + Vector3.up;
            Vector3 rayDirection = (player.transform.position + Vector3.up) - rayOrigin;
            float rayDistance = rayDirection.magnitude;
            rayDirection.Normalize();

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance))
            {
                //Debug.DrawLine(rayOrigin, hit.point);
                if (hit.collider.gameObject == player)
                {
                    playerInSight = true;
                }
                else
                {
                    playerInSight = false;
                }
            }
            enemyMovement.Rotation(direction);
            if (distance <= 10 && playerInSight)
            {
                if (agent!=null)
                {
                    if (agent.enabled)
                    {
                        agent.enabled = false;
                        obstacle.enabled = true;                                            
                    }
                  
                }
              
                    enemyAnimation.Attack2(true);
                        
            }
            else
            {
                enemyAnimation.Attack2(false);
                if (agent != null)
                {
                    obstacle.enabled = false;
                    agent.enabled = true;
                    if (IsPlayerOnNavMesh())
                    {
                        direction = player.transform.position;      
                    }
                    else
                    {
                        direction = SharedVariables.Instance.gatherPoint.transform.position;
                    }
                    enemyMovement.Rotation(direction);
                    enemyMovement.Movement(direction);
                    enemyAnimation.Movement(direction.magnitude);
                }
                else
                {
                    enemyMovement.Movement(direction, enemyStatus.speed);
                    enemyAnimation.Movement(direction.magnitude);
                }
     
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
        Vector3 direction = (player.transform.position - ThrowPos.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        ThrowPos.rotation = targetRotation;
        Instantiate(spear, ThrowPos.position,targetRotation);
        AudioController.instance.PlayOneShot(ThrowSound, 0.8f);
    }

    void GetRandomEnemy()
    {
        if (!isBoss&&!Ranged)
        {
            int randomEnemy = Random.Range(1, randomClothes.transform.childCount);
            randomClothes.transform.GetChild(randomEnemy).gameObject.SetActive(true);
        }
       
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
        if (!Ranged&&!isBoss)
        {
            Parent.Spawn3(this.transform.position);
            head.SetActive(false);
            cust.SetActive(false);
        }     
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

