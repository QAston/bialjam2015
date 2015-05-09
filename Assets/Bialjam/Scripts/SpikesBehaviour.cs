using UnityEngine;
using System.Collections;

public class SpikesBehaviour : MonoBehaviour {

	void OnTriggerEnter(Collider2D other) {
		var player = other.GetComponent<PlayerBehaviour> ();
		if (player != null) {
			player.DieCharacter();
		}
	}
}
