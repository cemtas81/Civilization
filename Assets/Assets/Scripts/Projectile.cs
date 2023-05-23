using UnityEngine;

public class Projectile : MonoBehaviour {

	[SerializeField] private float speed;
	private Rigidbody rigidbodyProjectile;

	void Start () {
		rigidbodyProjectile = GetComponent<Rigidbody>();
		
	}

	void FixedUpdate () {
		// moves the projectile forward using physics (rigidbody)
		rigidbodyProjectile.MovePosition(
			rigidbodyProjectile.position + (speed * Time.deltaTime * transform.forward));
	}

	// Destroy the projectile and the enemy
	void OnTriggerEnter (Collider other) {
		Quaternion rotation = Quaternion.LookRotation(-transform.forward);
		switch (other.tag) {
			case "Enemy":
				EnemyController enemy = other.GetComponent<EnemyController>();
				enemy.LoseHealth(1);
				enemy.BloodParticle(transform.position, rotation);
				break;
			case "Boss":
				BossController boss = other.GetComponent<BossController>();
				boss.LoseHealth(1);
				boss.BloodParticle(transform.position, rotation);
				break;
		}

		Destroy(gameObject);
	}
}
