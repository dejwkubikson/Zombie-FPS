using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadShotScript : MonoBehaviour {

	public Target target;

	// I had to make this script for the headshot, if I had two "Target" scripts assigned one to Head and the other one to RestOfBody the score for killing a zombie would add twice
	public void HeadShot()
	{
		GameObject gameControl = GameObject.Find("GameControlObject");
		GameControl gameControlScript = gameControl.GetComponent<GameControl> ();

		gameControlScript.headShotText.text = "Head shot bonus: + 50";
		// PlayerScript handles with displaying the score, therefore we can just add to it.
		gameControlScript.score += 50;
		gameControlScript.headShotCount++;

		// If we shot in the head we want to substract whole health
		target.TakeDamage (target.health);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
