using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTriggerScript : MonoBehaviour {

	public List<SpawnScript> spawnScripts;

	private int x = 0;
	private int z = 0;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {

			GetComponent<AudioSource> ().Play ();

			foreach (SpawnScript spawn in spawnScripts) {

				// Spawning the zombies next to each other.
				if (x < 9) {
					spawn.Spawn (new Vector3 (x, 0, z));
					x++;
				}

				// If we have 9 zombies in a row, we want to add another row one behind them.
				if (x >= 9) {
					z++;
					x = 0;
				}
			}
		}
	}
}
