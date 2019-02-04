using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScene : MonoBehaviour {

	public Text playerDetails;
	public Text statistics;
	public Text congratulation;

	public AudioClip endMenuMusic;
	public AudioClip buttonSound;

	private AudioSource sound;
	private float accuracy = 0;

	// Background music must play in a loop.
	public void PlayBackgroundMusic(){

		sound.clip = endMenuMusic;
		sound.loop = true;
		sound.Play ();
	}

	public void ReturnToMainMenu(){
		
		StartCoroutine (DelayMainMenu ());
	}

	public void QuitGame(){

		StartCoroutine (DelayExitApplication ());
	}

	// Because the background music is playing in a loop, we need to stop the loop so that the button plays only once.
	IEnumerator DelayMainMenu(){

		sound.clip = buttonSound;
		sound.loop = false;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.LoadLevel ("MainMenu");
	}

	// Because the background music is playing in a loop, we need to stop the loop so that the button plays only once.
	IEnumerator DelayExitApplication(){

		sound.clip = buttonSound;
		sound.loop = false;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.Quit ();
	}

	// Use this for initialization
	void Start () {

		sound = GetComponent<AudioSource>();
		PlayBackgroundMusic();

		// Making the mouse visible.
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		GameObject gameControl = GameObject.Find("GameControlObject");
		if(gameControl != null)
		{
			GameControl gameControlScript = gameControl.GetComponent<GameControl> ();

			if (gameControlScript.died)
				congratulation.text = "You died!";

			// Displaying the player's name and score.
			playerDetails.text = gameControlScript.playerName + ", your score is: " + gameControlScript.score.ToString () + "!";

			// Calculating the player's accuracy and rounding it to decimals.
			if (gameControlScript.shotsFired == 0) {
				accuracy = 0;
			} else {
				accuracy = (gameControlScript.targetHit / gameControlScript.shotsFired) * 100;
				accuracy = Mathf.Round (accuracy);
			}

			// Displaying the player's statistics.
			statistics.text = "Headshots: " + gameControlScript.headShotCount.ToString () + "\nZombies killed: " + gameControlScript.zombiesKilled.ToString () + "\nAccuracy: " + accuracy.ToString () + "%";
		}
	}
}
