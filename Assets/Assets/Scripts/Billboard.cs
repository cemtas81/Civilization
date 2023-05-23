using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour 
{
	private void Start()
	{
		GetComponent<Canvas>().worldCamera = Camera.main;
	}
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt(transform.position + Camera.main.transform.forward);
		
	}
}
