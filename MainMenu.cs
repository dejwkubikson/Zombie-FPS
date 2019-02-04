using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public AudioClip buttonSound;
	public AudioClip menuMusic;

	private AudioSource sound;

	public void LoadMainLevel(){
		// We need to play a full button sound, therefore a coroutine is used.
		StartCoroutine (DelayLoadMainLevel ());
	}

	public void LoadOptions(){
		// We need to play a full button sound, therefore a coroutine is used.
		StartCoroutine (DelayLoadOptions ());
	}

	public void QuitGame(){
		// We need to play a full button sound, therefore a coroutine is used.
		StartCoroutine (DelayExitApplication ());
	}

	// Background music must play in a loop.
	public void PlayBackgroundMusic(){

		sound.clip = menuMusic;
		sound.loop = true;
		sound.Play ();
	}

	// Because the background music is playing in a loop, we need to stop the loop so that the button plays only once.
	IEnumerator DelayLoadMainLevel(){

		sound.clip = buttonSound;
		sound.loop = false;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.LoadLevel ("MainLevel");
	}

	// Because the background music is playing in a loop, we need to stop the loop so that the button plays only once.
	IEnumerator DelayLoadOptions(){

		sound.clip = buttonSound;
		sound.loop = false;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.LoadLevel ("Options");
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

		GameObject gameControl = GameObject.Find("GameControlObject");
		// This prevents GameControl from duplicating
		if(gameControl == null)
		{
			gameControl = new GameObject("GameControlObject");
			gameControl.AddComponent<GameControl>();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
