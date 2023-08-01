using UnityEngine;
using DamageNumbersPro;
using UnityEngine.AI;

public class BarbEnemyCont2 : MonoBehaviour, IKillable
{
    [Header("Settings")]
    [SerializeField] private AudioClip deathSound, ThrowSound;
    [SerializeField] private GameObject bloodParticle, aidKit, spear;
    [SerializeField] private DamageNumber numberPrefab;
    [SerializeField] private GameObject head, cust, randomClothes;
    [SerializeField] private Transform ThrowPos;
    [SerializeField] private bool Ranged, isBoss;
    [SerializeField] private float probabilityAidKit = 0.08f;
    [Header("References")]
    [HideInInspector] public EnemySpawner EnemySpawner;

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
        GetRandomEnemy();
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
    }

    private void GetRandomEnemy()
    {
        if (!isBoss && !Ranged)
        {
            int randomEnemy = Random.Range(1, randomClothes.transform.childCount);
            randomClothes.transform.GetChild(randomEnemy).gameObject.SetActive(true);
        }
    }

    private void SetInitialSpeed()
    {
        enemyStatus.speed = Random.Range(2.6f, 3.1f);
    }

    private void SetRandomDamage()
    {
        random = isBoss ? 20 : Random.Range(5, 10);
    }

    private void FixedUpdate()
    {
        Vector3 pldirection=player.transform.position;
        Vector3 direction =pldirection - transform.position;
        direction.y = 0;
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (ShouldDestroyEnemy(distance))
        {
            DestroyEnemy();
            return;
        }

        if (!Ranged)
        {
            HandleMeleeEnemy(distance, direction);
        }
        else
        {
            HandleRangedEnemy(distance, direction);
        }
    }

    private bool ShouldDestroyEnemy(float distance)
    {
        return distance > 60 && agent == null;
    }

    private void DestroyEnemy()
    {
        Parent.spawnedPrefabs.Remove(this.gameObject);
        Destroy(gameObject);
        enabled = false;
    }

    private void HandleMeleeEnemy(float distance, Vector3 direction)
    {
        if (agent != null)
        {
            UpdateAgentAndObstacle(distance, direction);
        }
        else
        {
            enemyMovement.Movement(direction, enemyStatus.speed);
            enemyAnimation.Movement(direction.magnitude);
        }

        if (distance >= 2.1f)
        {
            enemyAnimation.Attack(false);
        }
        else
        {
            enemyMovement.Rotation(direction);
            enemyAnimation.Attack(true);
        }
    }

    private void UpdateAgentAndObstacle(float distance, Vector3 direction)
    {
        if (!agent.enabled)
        {
            obstacle.enabled = false;
            agent.enabled = true;
        }

        if (IsPlayerOnNavMesh())
        {
            direction = player.transform.position;
        }
        else
        {
            direction = SharedVariables.Instance.gatherPoint.transform.position;
        }

        enemyMovement.Movement(direction);
        enemyAnimation.Movement(direction.magnitude);

        if (distance < 2.1f)
        {
            agent.velocity = Vector3.zero;
            agent.enabled = false;
            obstacle.enabled = true;
        }
    }

    private void HandleRangedEnemy(float distance, Vector3 direction)
    {
        UpdatePlayerInSight();

        enemyMovement.Rotation(direction);

        if (distance <= 10 && playerInSight)
        {
            if (agent != null)
            {
                agent.enabled = false;
                obstacle.enabled = true;
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
            }

            enemyMovement.Movement(direction, enemyStatus.speed);
            enemyAnimation.Movement(direction.magnitude);
        }
    }

    private void UpdatePlayerInSight()
    {
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 rayDirection = (player.transform.position + Vector3.up) - rayOrigin;
        float rayDistance = rayDirection.magnitude;
        rayDirection.Normalize();

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance))
        {
            playerInSight = hit.collider.gameObject == player;
        }
    }

    private bool IsPlayerOnNavMesh()
    {
        return NavMesh.SamplePosition(player.transform.position, out _, 1f, NavMesh.AllAreas);
    }

    public void LoseHealth(int damage)
    {
        enemyStatus.health -= damage;
        if (enemyStatus.health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        PrepareForDestruction();
        screenController.UpdateDeadZombiesCount();
        HandleNonRangedNonBoss();
        Parent.spawnedPrefabs.Remove(this.gameObject);
        AudioController.instance.PlayOneShot(deathSound, Random.Range(0.2f, 0.9f));
        InstantiateAidKit(probabilityAidKit);
    }

    private void PrepareForDestruction()
    {
        Destroy(gameObject, 1.5f);
        enemyAnimation.Die();
        enemyMovement.Die();

        if (agent != null)
        {
            agent.enabled = false;
        }

        enabled = false;
    }

    private void HandleNonRangedNonBoss()
    {
        if (!Ranged &&!isBoss)
        {
            Parent.Spawn3(this.transform.position);
            head.SetActive(false);
            cust.SetActive(false);
        }
    }

    private void InstantiateAidKit(float probability)
    {
        if (Random.value <= probability)
            Instantiate(aidKit, transform.position, Quaternion.identity);
    }
}
