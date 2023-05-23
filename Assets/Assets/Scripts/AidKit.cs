using UnityEngine;

public class AidKit : MonoBehaviour {

	private int healAmount = 15;
	private int lifeTime = 5;

	void Start () {
		Destroy(gameObject, lifeTime);
	}

	void OnTriggerEnter (Collider other) {
		if (other.CompareTag("Player")) {
			other.GetComponent<PlayerController>().HealHealth(healAmount);
			Destroy(gameObject);
		}
	}
}
