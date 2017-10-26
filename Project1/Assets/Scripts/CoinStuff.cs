using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStuff : MonoBehaviour {
	
	public int points = 5;
	
	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Triggered");
		Destroy(gameObject);
	}
}
