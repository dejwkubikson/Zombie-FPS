using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

	public GameObject zombiePrefab;

	public void Spawn(Vector3 spawnPosition)
	{
		GameObject zombie = (GameObject)Instantiate (zombiePrefab, transform.position + spawnPosition, Quaternion.identity);
		zombie.name = "Zombie";
	}
}
