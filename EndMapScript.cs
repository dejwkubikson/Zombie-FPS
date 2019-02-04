using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndMapScript : MonoBehaviour {

	void OnTriggerExit(Collider other){
		
		if (other.gameObject.tag == "Player")
			StartCoroutine (PlayAndLoadEndScene ());
	}

	IEnumerator PlayAndLoadEndScene(){

		GetComponent<AudioSource> ().Play ();
		yield return new WaitForSeconds (GetComponent<AudioSource> ().clip.length);
		Application.LoadLevel("EndScene");
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
