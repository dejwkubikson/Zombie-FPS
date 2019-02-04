using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickUpScript : MonoBehaviour {

	public GameObject gun;
	public GunScript gunScript;
	public Player player;
	public Text notificationText;

	private float rotationSpeed = 10.0f;
	private AudioSource sound;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
	}
		
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Player") {

			if (this.gameObject.tag == "GunToPickUp") {
				notificationText.text = "You picked up a gun";
				StartCoroutine (PlaySoundAndDestroy ());
				gun.SetActive (true);
			}

			if (this.gameObject.tag == "AmmoBox") {
				notificationText.text = "You picked up 40 bullets";
				gunScript.ammoReserve += 40;
				StartCoroutine (PlaySoundAndDestroy ());
			}

			if (this.gameObject.tag == "HealthPack") {
				if (player.health < 100) {

					notificationText.text = "You picked up aid kit";

					if (player.health + 30 > 100)
						player.health = 100;
					else
						player.health += 30;
					
					StartCoroutine (PlaySoundAndDestroy ());
				}
			}
		}
	}

	IEnumerator PlaySoundAndDestroy()
	{
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length-0.1f);
		Destroy (this.gameObject);
	}

	// Update is called once per frame
	void Update () {

		// This is used so that the ammo boxes will not rotate, only the gun, I do not need to write a different script for them
		if(gameObject.tag == "GunToPickUp")
			transform.Rotate (Vector3.up * Time.deltaTime * rotationSpeed);
		
	}
}
