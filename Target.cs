using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Target : MonoBehaviour {

	public int health = 100;

	public GameObject fullBody;
	public GameObject body;
	public AudioClip zombieDieSound;
	public AudioClip[] zombieSounds;

	private float timeElapsed = 0.0f;
	private AudioSource sound;
	private int randomSound = 0;
	private int numClips = 0;
	private float distance = 0;
	private bool ableToAttack = true;

	public void TakeDamage(int amount)
	{
		GameObject gameControl = GameObject.Find("GameControlObject");
		GameControl gameControlScript = gameControl.GetComponent<GameControl> ();

		gameControlScript.targetHit++;
		health -= amount;
		if (health <= 0) 
		{
			Die ();
		}
	}
		
	public virtual void Die()
	{
		GameObject gameControl = GameObject.Find("GameControlObject");
		GameControl gameControlScript = gameControl.GetComponent<GameControl> ();

		gameControlScript.notificationText.text = "Zombie killed: + 100";
		// GameControl handles with displaying the score, therefore we can just add to it.
		gameControlScript.score += 100;
		gameControlScript.zombiesKilled++;

		StartCoroutine (PlaySoundAndDie ());
	}

	public IEnumerator PlaySoundAndDie()
	{
		sound.clip = zombieDieSound;
		sound.Play();
		yield return new WaitForSeconds (sound.clip.length);
		Destroy (fullBody);
	}

	// Use this for initialization
	void Start () {
	
		sound = GetComponent<AudioSource>();

		// Getting the zombieSounds array length to know from what range to choose the music.
		numClips = zombieSounds.Length;
	}

	public IEnumerator AttackPlayer()
	{
		ableToAttack = false;
		GameObject player = GameObject.Find("Player");
		Player playerScript = player.GetComponent<Player> ();
		playerScript.TakeDamage (15);
		yield return new WaitForSeconds (0.8f);

		if(playerScript.health > 0)
			ableToAttack = true;
	}

	// Update is called once per frame
	void Update () {

		// Making the zombie look at the player.
		GameObject player = GameObject.Find("Player");
		fullBody.transform.LookAt (player.transform.position * 3.0f);

		// Making zombie move towards the player.
		transform.position = Vector3.MoveTowards (transform.position, player.transform.position, Time.deltaTime);

		// This is the distance between the player and the zombie.
		distance = Vector3.Distance (player.transform.position, gameObject.transform.position);

		// If the zombie is near the player he attacks him and takes 15 player's health.
		if (distance < 2.0f && ableToAttack) {
			StartCoroutine (AttackPlayer ());
		}

		timeElapsed = Time.time;

		// One of two sounds is played every 10 seconds from the zombie. It is choosen randomly.
		if (Mathf.Round (timeElapsed) % 10 == 0) {
			randomSound = Random.Range (0, numClips);
			sound.clip = zombieSounds [randomSound];
			sound.PlayOneShot (sound.clip, 0.8f);
		}
	}
}
