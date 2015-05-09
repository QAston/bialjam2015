using UnityEngine;
using System.Collections;

public class SpikesBehaviour : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log (other.name);
		var player = PlayerBehaviour.GetForCharater(other.gameObject);
		if (player != null) {
			player.DieCharacter();
		}
	}
}
