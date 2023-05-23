using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour, IKillable{

	[SerializeField]private GameObject aidKitPrefab;
	[SerializeField] private Slider bossSlider;
	[SerializeField] private Image sliderImage;
	[SerializeField] private Color maxHealthColor;
	[SerializeField] private Color minHealthColor;
	[SerializeField] private GameObject bloodParticle;
	
	private Transform player;
	private NavMeshAgent agent;
	private Status bossStatus;
	private CharacterAnimation bossAnimation;
	private CharacterMovement bossMovement;
	
	private void Start() {
		player = GameObject.FindWithTag("Player").transform;
		agent = GetComponent<NavMeshAgent>();
		bossStatus = GetComponent<Status>();
		agent.speed = bossStatus.speed;
		bossAnimation = GetComponent<CharacterAnimation>();
		bossMovement = GetComponent<CharacterMovement>();
		bossSlider.maxValue = bossStatus.initialHealth;
		UpdateInterface();
	}

	private void Update() {
		agent.SetDestination(player.position);
		bossAnimation.Movement(agent.velocity.magnitude);

		if (agent.hasPath) {
			bool closeToPlayer = agent.remainingDistance <= agent.stoppingDistance;
			if (closeToPlayer) {
				bossAnimation.Attack(true);
				Vector3 direction = player.position - transform.position;
				bossMovement.Rotation(direction);
			}
			else {
				bossAnimation.Attack(false);
			}
		}
	}

	private void AttackPlayer() {
		int damage = Random.Range(30, 40);
		player.GetComponent<PlayerController>().LoseHealth(damage);
	}

	public void LoseHealth(int damage) {
		bossStatus.health -= damage;
		UpdateInterface();
		if (bossStatus.health <= 0)
			Die();
	}

	public void Die() {
		Destroy(gameObject, 2);
		bossAnimation.Die();
		bossMovement.Die();
		Instantiate(aidKitPrefab, transform.position, Quaternion.identity);
		enabled = false;
		agent.enabled = false;
	}

	public void BloodParticle(Vector3 position, Quaternion rotation) {
		Instantiate(bloodParticle, position, rotation);
	}
	
	private void UpdateInterface() {
		bossSlider.value = bossStatus.health;
		float healthPercent = (float)bossStatus.health / bossStatus.initialHealth;
		Color healthColor = Color.Lerp(minHealthColor, maxHealthColor, healthPercent);
		sliderImage.color = healthColor;
	}
}
