using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public int score = 0;
	public int zombiesKilled = 0;
	public int headShotCount = 0;
	// shotsFired and targetHit variables must be float to avoid integer division, they are divided in the EndScene.
	public float shotsFired = 0;
	public float targetHit = 0;
	public string playerName =  "";
	// Volume value cannot be 0 because the user won't hear the sound without adjusting the volume slider.
	public float volume = 1.0f;
	public bool died = false;

	public Text notificationText;
	public Text headShotText;
	public Text scoreText;

	private string sceneName;

	// This function is used in order to prevent a certain notification display all the time.
	IEnumerator DelayNotification(){
		// 3 seconds after a notification appears it will be removed.
		yield return new WaitForSeconds (3);
		notificationText.text = "";
	}
		
	IEnumerator DelayHeadshotText(){
		// 3 seconds after a notification appears it will be removed.
		yield return new WaitForSeconds (3);
		headShotText.text = "";
	}

	// Use this for initialization
	void Start () {
		// To prevent from loosing the GameControl settings (name, volume) it cannot be destroyed when loading another scene.
		DontDestroyOnLoad(this);

		// The volume adjusted by user
		AudioListener.volume = volume;

		// I am using this in order to check if the notification needs to be removed (it can only be removed in the Main Level) 
		// and if the player had a chance to put his name in - without this his name would be Unnamed in the input field in options scene.
		Scene currentScene = SceneManager.GetActiveScene ();
		sceneName = currentScene.name;

		// If the player does not put any name he will be shown as Unnamed;
		if (sceneName == "MainLevel") {
			if (playerName == "")
				playerName = "Unnamed";
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (sceneName == "MainLevel") {
			// This function resets the notification so that it won't be hanging on the screen for the whole game.
			if (notificationText.text != "")
				StartCoroutine (DelayNotification ());

			// This function resets the notification so that it won't be hanging on the screen for the whole game.
			if (headShotText.text != "")
				StartCoroutine (DelayHeadshotText ());

			scoreText.text = playerName + "'s Score: " + score.ToString();
		}
	}
}
