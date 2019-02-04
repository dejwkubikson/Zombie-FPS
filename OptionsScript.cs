using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsScript : MonoBehaviour {

	public string nameText = "";
	public Text volumeText;
	public InputField nameInputField;
	public Slider volumeSlider;

	public AudioClip buttonSound;
	public AudioClip menuMusic;

	private AudioSource sound;

	public void LoadMainMenu(){

		// Updating the GameControl script in order to adjust the volume and the name user wants.
		GameObject gameControl = GameObject.Find("GameControlObject");
		// This is made just in case, the gameControl should exist because it's made in the MainMenu start function.
		if(gameControl != null)
		{
			GameControl gameControlScript = gameControl.GetComponent<GameControl> ();
			gameControlScript.playerName = nameText;
			gameControlScript.volume = volumeSlider.value;
		}

		// We need to play a full button sound, therefore a coroutine is used.
		StartCoroutine (DelayLoadMainMenu ());
	}

	public void SetName(string name){

		nameText = name;
	}

	// Background music must play in a loop.
	public void PlayBackgroundMusic(){

		sound.clip = menuMusic;
		sound.loop = true;
		sound.Play ();
	}

	// Because the background music is playing in a loop, we need to stop the loop so that the button plays only once.
	IEnumerator DelayLoadMainMenu(){

		sound.clip = buttonSound;
		sound.loop = false;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.LoadLevel ("MainMenu");
	}

	// Function to adjust the volume.
	public void ChangeAudioVolume(float volume)
	{
		AudioListener.volume = volume;
	}

	// Use this for initialization
	void Start () {

		// If the player will play continuously, he will have the last name he used.
		GameObject gameControl = GameObject.Find("GameControlObject");
		GameControl gameControlScript = gameControl.GetComponent<GameControl>();
		nameInputField.text = gameControlScript.playerName;

		sound = GetComponent<AudioSource>();
		PlayBackgroundMusic();

		volumeSlider.value = AudioListener.volume;
	}

	// Update is called once per frame
	void Update () {

		// Displays the volume in % format for example 50%.
		volumeText.text = "Volume: " + Mathf.Floor ((volumeSlider.value * 100)) + "%";

		ChangeAudioVolume (volumeSlider.value);
	}
}
