using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public Text healthText;
	public int health = 100;
	public GameControl gameControl;
	public AudioClip dieSound;
	public AudioClip hitSound;

	private AudioSource sound;

	public void TakeDamage(int amount)
	{
		sound.clip = hitSound;
		sound.Play ();
		health -= amount;
		if (health <= 0) 
		{
			health = 0;
			Die ();
		}
	}

	public virtual void Die()
	{
		gameControl.died = true;
		StartCoroutine (PlaySoundAndDie ());
	}

	IEnumerator PlaySoundAndDie(){

		sound.clip = dieSound;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Application.LoadLevel("EndScene");
	}

	// Use this for initialization
	void Start () {

		sound = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

		healthText.text = "HEALTH + \n" + health.ToString ();

		if (health < 60 && health > 30)
			healthText.color = new Color32 (255, 111, 0, 255);

		if (health <= 30)
			healthText.color = new Color32 (255, 0, 0, 255);
	}
}
