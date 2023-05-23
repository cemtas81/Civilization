using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour {

	[SerializeField] private GameObject bossPrefab;
	[SerializeField] private Transform[] spawnPositions;
	private float nextSpawnTime;
	private float timeBetweenSpawns = 60;
	private ScreenController screenController;
	private Transform player;
	
	// Use this for initialization
	void Start () {
		nextSpawnTime = timeBetweenSpawns;
		screenController = GameObject.FindObjectOfType<ScreenController>();
		player = GameObject.FindWithTag("Player").transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > nextSpawnTime) {
			Vector3 spawnPosition = GetSpawnPosition();
			Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
			screenController.ShowBossText();
			nextSpawnTime += timeBetweenSpawns;
		}
	}
	
	void OnDrawGizmos () {
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, 1.5f);
	}

	private Vector3 GetSpawnPosition() {
		Vector3 positionWithBiggerDistance = Vector3.zero;
		float biggestDistance = 0;
		foreach (Transform position in spawnPositions) {
			float distanceBetweenPlayer = Vector3.Distance(position.position, player.position);
			if (distanceBetweenPlayer > biggestDistance) {
				biggestDistance = distanceBetweenPlayer;
				positionWithBiggerDistance = position.position;
			}

		}
		return positionWithBiggerDistance;
	}
}
