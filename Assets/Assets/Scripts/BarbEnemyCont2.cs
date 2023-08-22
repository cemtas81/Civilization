using UnityEngine;
using UnityEngine.AI;

public class BarbEnemyCont2 : MonoBehaviour, IKillable
{
    [Header("Settings")]
    [SerializeField] private AudioClip deathSound, ThrowSound;
    [SerializeField] private GameObject bloodParticle, aidKit, spear;
    [SerializeField] private GameObject head, cust, randomClothes;
    [SerializeField] private Transform ThrowPos;
    [SerializeField] private bool Ranged, isBoss,towered;
    [SerializeField] private float probabilityAidKit = 0.08f;
    [Header("References")]
    [HideInInspector] public EnemySpawner EnemySpawner;
    private AudioSource source;
    private Status enemyStatus;
    private GameObject player;
    private CharacterMovement enemyMovement;
    private CharacterAnimation enemyAnimation;
    private BarbScreenCont screenController;
    private MySolidSpawner Parent;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    private bool playerInSight;
    private int random;

    private void Awake()
    {
        InitializeComponents();
        if (!isBoss && !Ranged) GetRandomEnemy();
        SetInitialSpeed();
        SetRandomDamage();
    }

    private void InitializeComponents()
    {
        obstacle = GetComponent<NavMeshObstacle>();
        player = SharedVariables.Instance.playa;
        enemyMovement = GetComponent<CharacterMovement>();
        enemyAnimation = GetComponent<CharacterAnimation>();
        enemyStatus = GetComponent<Status>();
        screenController = SharedVariables.Instance.screenCont;
        Parent = SharedVariables.Instance.spawner;
        agent = GetComponent<NavMeshAgent>();
        Parent.spawnedPrefabs.Add(this.gameObject);
        enabled = true;
        source = SharedVariables.Instance.audioS;
    }

    private void GetRandomEnemy()
    {
        int randomEnemy = Random.Range(1, randomClothes.transform.childCount);
        randomClothes.transform.GetChild(randomEnemy).gameObject.SetActive(true);
    }

    private void SetInitialSpeed() => enemyStatus.speed = !isBoss ? Random.Range(2.6f, 3.1f) : enemyStatus.speed;

    private void SetRandomDamage() => random = isBoss? 20 : Random.Range(5, 10);

    private void FixedUpdate()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if (screenController.deadCam.enabled) { StopEnemy(); return; }

        if (ShouldDestroyEnemy(distance)) { DestroyEnemy(); return; }

        if (!Ranged) HandleMeleeEnemy(distance, direction);
        else HandleRangedEnemy(distance, direction);
    }

    private bool ShouldDestroyEnemy(float distance) => distance >= 60 && agent == null&&!towered;

    private void DestroyEnemy()
    {
        Parent.spawnedPrefabs.Remove(this.gameObject);
        Destroy(gameObject);
        enabled = false;
    }

    private void UpdateAgentAndObstacle(float distance, Vector3 direction)
    {
        if (!agent.enabled) { obstacle.enabled = false; agent.enabled = true; }

        direction = IsPlayerOnNavMesh() ? player.transform.position : SharedVariables.Instance.gatherPoint.transform.position;
        enemyMovement.Movement(direction);
        enemyAnimation.Movement(direction.magnitude);      
        enemyAnimation.Attack(false);
        if (distance <= 2.1f)
        {
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            obstacle.enabled = true;
            enemyAnimation.Attack(true); 
            direction.y = 0;
            enemyMovement.Rotation(direction - transform.position);
        }
    }

    private void HandleMeleeEnemy(float distance, Vector3 direction)
    {
        if (agent != null) UpdateAgentAndObstacle(distance, direction);
        else
        {
            if (!IsPlayerOnNavMesh())
            {
                if (distance >= 2.1f) { enemyMovement.Movement(direction, enemyStatus.speed); enemyAnimation.Movement(direction.magnitude); 
                    enemyAnimation.Attack(false); enemyMovement.Rotation(direction); }
                else { enemyMovement.Rotation(direction); enemyAnimation.Attack(true); enemyMovement.StandStill(); }       
            }
            else
                StopEnemy();
        }
    }

    private void HandleRangedEnemy(float distance, Vector3 direction)
    {
        UpdatePlayerInSight();
        enemyMovement.Rotation(direction);
        if (!agent && IsPlayerOnNavMesh()) { StopEnemy(); return; }

        if (distance <= 10 && playerInSight)
        {
            if (agent != null) { agent.enabled = false; obstacle.enabled = true; }
      
            enemyAnimation.Attack2(true); enemyMovement.StandStill();
        }
        else
        {
            enemyAnimation.Attack2(false);
            if (agent != null) UpdateAgentAndObstacle(distance, direction);

            else { enemyMovement.Movement(transform.forward, enemyStatus.speed); enemyAnimation.Movement(direction.magnitude); }
     
        }
    }

    private void UpdatePlayerInSight()
    {
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = (player.transform.position + Vector3.up) - rayOrigin;
        float rayDistance = rayDirection.magnitude;
        rayDirection.Normalize();
        playerInSight = Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance) && hit.collider.gameObject == player;
    }

    private bool IsPlayerOnNavMesh() => NavMesh.SamplePosition(player.transform.position, out _, 1f, NavMesh.AllAreas);

    public void LoseHealth(int damage)
    {
        enemyStatus.health -= damage;
        if (enemyStatus.health <= 0) Die();
    }

    public void BloodParticle(Vector3 position, Quaternion rotation) => Instantiate(bloodParticle, position, rotation);

    public void Die()
    {
        PrepareForDestruction();
        screenController.UpdateDeadZombiesCount();
        HandleNonRangedNonBoss();
        Parent.spawnedPrefabs.Remove(this.gameObject);
        source.PlayOneShot(deathSound, Random.Range(0.2f, 0.9f));
        InstantiateAidKit(probabilityAidKit);
    }

    private void PrepareForDestruction()
    {
        Destroy(gameObject, 1.5f);
        enemyAnimation.Die();
        enemyMovement.Die();
        if (agent != null) agent.enabled = false;
        enabled = false;
    }

    private void HandleNonRangedNonBoss()
    {
        if (!Ranged && !isBoss) { Parent.Spawn3(this.transform.position); head.SetActive(false); cust.SetActive(false); }
    }

    private void InstantiateAidKit(float probability)
    {
        if (Random.value <= probability) Instantiate(aidKit, transform.position, Quaternion.identity);
    }

    void AttackPlayer() => player.GetComponent<BarbCont2>().LoseHealth(random);

    void AttackPlayer2()
    {
        Vector3 direction = (player.transform.position - ThrowPos.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        ThrowPos.rotation = targetRotation;
        Instantiate(spear, ThrowPos.position, targetRotation);
        source.PlayOneShot(ThrowSound, 0.8f);
    }
    private void StopEnemy()
    {
        if (agent != null && agent.enabled) { agent.velocity = Vector3.zero; }
     
        enemyAnimation.Attack(false);
        enemyAnimation.Attack2(false);
        enemyMovement.Movement(Vector3.zero);
        enemyAnimation.Movement(0);
    }
}
