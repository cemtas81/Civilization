using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField] private GameObject enemyPrefab;
	[SerializeField] private float instantiateTime = 1;
	[SerializeField] private LayerMask enemyLayer;
	private float instantiationDistance = 3;
	private float playerDistance = 20;
	private float timeCounter = 0;
	private GameObject player;
	private int maxAliveEnemiesAmount = 2;
	private int aliveEnemiesAmount;
	private float nextDifficultyIncreaseTime = 5;
	private float difficultyIncreaseCounter;

	void Start () {
		player = GameObject.FindWithTag("Player");
		difficultyIncreaseCounter = nextDifficultyIncreaseTime;
		for (int i = 0; i < maxAliveEnemiesAmount; i++) {
			StartCoroutine(InstantiateNewEnemy());
		}
	}

	// instantiates a new enemy each second
	void Update () {

		bool canInstantiateEnemy = Vector3.Distance(transform.position, 
			                            player.transform.position) > playerDistance;
		
		if (canInstantiateEnemy && aliveEnemiesAmount < maxAliveEnemiesAmount) {
			timeCounter += Time.deltaTime;

			if (timeCounter >= instantiateTime) {
				StartCoroutine(InstantiateNewEnemy());
				timeCounter = 0;
			}	
		}

		if (Time.timeSinceLevelLoad >= difficultyIncreaseCounter) {
			maxAliveEnemiesAmount++;
			difficultyIncreaseCounter += nextDifficultyIncreaseTime;
		}
	}

	private IEnumerator InstantiateNewEnemy () {
		Vector3 position = GetRandomPosition();
		Collider[] colliders = Physics.OverlapSphere(position, 1, enemyLayer);
		
		while (colliders.Length > 0) {
			position = GetRandomPosition();
			colliders = Physics.OverlapSphere(position, 1, enemyLayer);
			yield return null;
		}
		
		EnemyController enemy = Instantiate(enemyPrefab, position, transform.rotation).GetComponent<EnemyController>();
		enemy.EnemySpawner = this;
		aliveEnemiesAmount++;
	}

	private Vector3 GetRandomPosition () {
		Vector3 position = Random.insideUnitSphere * instantiationDistance;
		position += transform.position;
		position.y = 0;
		return position;
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, instantiationDistance);
	}

	public void DecreaseAliveEnemiesAmount() {
		aliveEnemiesAmount--;
	}
}
