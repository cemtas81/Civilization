using UnityEngine;

public class AudioController : MonoBehaviour {

	// works as a simplified singleton pattern
	public static AudioSource instance;

	private AudioSource audioSource;

	void Awake () {
		audioSource = GetComponent<AudioSource>();
		instance = audioSource;
	}

}
