using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingMusic : MonoBehaviour {

	public Text notificationText;
	public AudioClip[] backgroundMusic;

	private int numClips = 0;
	private AudioSource sound;

	private int i = 0;
	private int playClip = 0;
	private bool inTrigger = false;

	// When player approaches the music object he will be notified to press E to change the music.
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player")
			inTrigger = true;
		
		notificationText.text = "Press E to change music";
	}

	// If the player exits the trigger we want to clear the notification.
	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			inTrigger = false;

			notificationText.text = "";
		}
	}

	// To change the music player needs to press E.
	void ChangeMusic()
	{
		if ((Input.GetKey (KeyCode.E))) {
			Debug.Log (i);
			if (i < (numClips - 1)) {
				StartCoroutine (DelayMusicChange (playClip + 1));
			} else {
				StartCoroutine (DelayMusicChange (0));
			}
		}
	}
		
	// Music in the array will change after the previous one has ended. Music is looped.
	IEnumerator DelayMusicChange(int playClip)
	{
		for (i = playClip; i < numClips; i++) {

			sound.clip = backgroundMusic [i];
			sound.Play ();
			yield return new WaitForSeconds (sound.clip.length);

			// If we play all the music clips, we want to start playing them all over again. Therefore, to make this loop an infinite loop we need to change the value of i.
			if (i == (numClips - 1))
				i = -1; // It is -1 because after completing the loop it will go i++, it will be 0 at the start of the loop.
		}

	}

	// Function that plays the background music at the start of the game.
	public void PlayBackgroundMusic()
	{
		StartCoroutine (DelayMusicChange (playClip));
	}

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();

		// Getting the backgroundMusic array length to know how many music clips there are.
		numClips = backgroundMusic.Length;

		PlayBackgroundMusic ();
	}
	
	// Update is called once per frame
	void Update () {
		
		// Checking if the player is in the trigger to change music.
		if (inTrigger) {
			ChangeMusic ();
		}
	}
}
