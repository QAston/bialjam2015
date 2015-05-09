using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class LeverBehaviour : MonoBehaviour {

	public GameObject obj;

	public void OnTriggerEnter2D(Collider2D collider) { 
		Debug.Log ("collision");
		var player = collider.GetComponent<PlatformerCharacter2D> ();
		if (player != null) {
			Animator anim = obj.GetComponent<Animator>();
			anim.enabled = !anim.enabled;
		}
	}
}