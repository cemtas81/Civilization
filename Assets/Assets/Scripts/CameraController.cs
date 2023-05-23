using UnityEngine;

public class CameraController : MonoBehaviour {

	[SerializeField] private Transform player;

	private Vector3 distance;

	void Start () {
		// calculates the distance between this camera and the player
		distance = transform.position - player.position;
	}

	void LateUpdate () {
		// sets the new camera position 
		transform.position = player.position + distance;
	}
}
