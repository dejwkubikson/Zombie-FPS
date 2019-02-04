using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetParticleEffect : Target {

	public GameObject bloodBurst;

	public override void Die()
	{
		GameObject gameControl = GameObject.Find("GameControlObject");
		GameControl gameControlScript = gameControl.GetComponent<GameControl> ();

		gameControlScript.zombiesKilled++;

		Instantiate (bloodBurst, body.transform.position, Quaternion.identity);
		gameControlScript.notificationText.text = "";
		gameControlScript.notificationText.text = "Zombie killed: + 100";
		// GameControl handles with displaying the score, therefore we can just add to it.
		gameControlScript.score += 100;
		StartCoroutine (PlaySoundAndDie ());
	}

}
