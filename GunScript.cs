using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunScript : MonoBehaviour {

	public Text notificationText;
	public Text ammoText;
	public Image crossHair;
	public AudioClip gunShotSound;
	public AudioClip gunReloadSound;
	public GameObject gun;
	public Camera fpsCam;
	public ParticleSystem muzzleFlash;
	public GameObject imageMuzzle;
	public GameObject impactEffect;

	private const int clipCapacity = 30;
	public int ammoReserve = 0;
	private int currentClip = 0;
	private int ammoToFill = 0;

	private Vector3 gunAimPosition = new Vector3(-0.01f, -0.35f, -0.35f); 
	private Vector3 gunHipPosition = new Vector3(0.58f, -0.75f, 0.87f);
	private float aimSpeed = 10.0f; // This is the speed of changing from hip to aim sight
	private bool isAbleToShoot = true;
	private bool isReloading = false; // This is made to be sure that the player does not shoot when reloading
	private AudioSource sound;
	private int muzzlePos = 0;
	private int damage = 30;
	private float impactForce = 40.0f;

	IEnumerator DelayFire()
	{
		isAbleToShoot = false;
		yield return new WaitForSeconds (0.17f);
		isAbleToShoot = true;
	}

	IEnumerator DelayMuzzleFlash()
	{
		imageMuzzle.SetActive (true);
		imageMuzzle.transform.Rotate (new Vector3 (0, 0, muzzlePos));
		yield return new WaitForSeconds (0.17f);
		imageMuzzle.SetActive (false);
		muzzlePos++;
	}

	void Shoot()
	{
		if (currentClip > 0) {
		
			// Counting the player's shots and adding the value to the gameControlScript's shotsFired variable.
			GameObject gameControl = GameObject.Find("GameControlObject");
			GameControl gameControlScript = gameControl.GetComponent<GameControl> ();
			gameControlScript.shotsFired++;

			GetComponent<AudioSource> ().PlayOneShot (gunShotSound);
			muzzleFlash.Play ();
			StartCoroutine (DelayMuzzleFlash ());
			currentClip--;

			// This is used to store the results of a raycast, it will be stored in this object
			RaycastHit hit = new RaycastHit ();

			if (Physics.Raycast (fpsCam.transform.position, fpsCam.transform.forward, out hit)) {

				// Assining the target script
				Target target = hit.transform.GetComponent<Target> ();
				HeadShotScript headShotScript = hit.transform.GetComponent<HeadShotScript> ();

				// Checking if the target and the headShotScript exists. 
				if (target != null || headShotScript != null) {
					// If we hit the head we want to pass it to the headShotScript script
					if (hit.transform.tag == "Head") {
						headShotScript.HeadShot ();
					}
					target.TakeDamage (damage);
				}

				// Applying force only if the object has a rigidbody.
				if (hit.rigidbody != null) {
					hit.rigidbody.AddForce (fpsCam.transform.forward * impactForce);
				}

				// Instantiating impact force when shooting (e.g. shooting the ground).
				GameObject impactGO = Instantiate (impactEffect, hit.point, Quaternion.LookRotation (hit.normal));

				// The code above clones the impact effect, therefore to not loose FPS we need to delete it after 1 second.
				Destroy (impactGO, 1f);
			}
		} else {
			notificationText.text = "Press R to reload your gun!";
		}
	}

	IEnumerator PlaySoundAndReload()
	{
		isReloading = true;
		sound.clip = gunReloadSound;
		sound.Play ();
		yield return new WaitForSeconds (sound.clip.length);
		Reload ();
		isReloading = false;
	}

	void Reload()
	{
		if (gun.activeSelf) {
			// We don't need to check if the player has any ammo or if he is eligble for a reload because it's already checked in the if in Update()
			ammoToFill = 30 - currentClip;

			// If there is less ammo in reserve than the player needs.
			if ((ammoReserve - ammoToFill) < 0) {
				currentClip += ammoReserve;
				ammoReserve = 0;
			} else {
				ammoReserve -= ammoToFill;
				currentClip = 30;
			}
		}
	}

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();

		crossHair.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		
		// If the player picks up the gun we turn on the crosshair and the amount of ammunition he has.
		if (gun.activeSelf) {
			ammoText.text = "AMMO III \n" + currentClip.ToString () + " / " + ammoReserve.ToString (); 
			crossHair.enabled = true;
		}

		// If the user will hold the RMB he will zoom
		if (Input.GetButton ("Fire2")) {

			// Getting rid of the crosshair when aiming
			crossHair.enabled = false;

			// Assigning the position of the gun when aiming, Slerp "slows" the transformation
			transform.localPosition = Vector3.Slerp (transform.localPosition, gunAimPosition, aimSpeed * Time.deltaTime);

			// Adjusting the camera's field of view to actually have the zoom effect
			fpsCam.fieldOfView = 50;


		} else {

			// There is no need to turn on the crosshair because it is already turned on in the if(gun.activeSelf), it is just turned off when aiming, so when we stop it automatically turns on.

			// Assigning the position of the gun when not aiming
			transform.localPosition = Vector3.Slerp (transform.localPosition, gunHipPosition, aimSpeed * Time.deltaTime);

			// If the user is not holding the RMB we need to delete the effect caused by the zoom
			fpsCam.fieldOfView = 60;
		}

		// Checking if there actually is something to reload to play the sound and if the player is not reloading at the moment.
		if (Input.GetKey (KeyCode.R) && currentClip < clipCapacity && ammoReserve > 0 && !(isReloading)) 
			StartCoroutine(PlaySoundAndReload());
		
		if (Input.GetButton ("Fire1") && isAbleToShoot && !(isReloading)) {
			StartCoroutine (DelayFire ());
			Shoot ();	
		}

	}
}

