using UnityEngine;

public class Status : MonoBehaviour {

	public int initialHealth = 100;
	[HideInInspector] public int health;
	public float speed = 5;

	// Use this for initialization
	void Awake ()
	{
		health = initialHealth;
	}
		
}
