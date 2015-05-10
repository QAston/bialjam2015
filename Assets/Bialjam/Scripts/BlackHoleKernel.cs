using UnityEngine;
using System.Collections;

public class BlackHoleKernel : MonoBehaviour {
	
	void OnTriggerEnter2D (Collider2D other) {
		var player = PlayerBehaviour.GetForCharater(other.gameObject);
		if (player != null && player.GetState() == PlayerBehaviour.State.GHOST)
		{
			Application.LoadLevel (Application.loadedLevel);
		}
	}
	
}
