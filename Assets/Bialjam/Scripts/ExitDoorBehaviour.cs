using UnityEngine;
using System.Collections;

public class ExitDoorBehaviour : MonoBehaviour {

	public string targetLevel;

	void OnTriggerEnter2D(Collider2D other) {
		var player = PlayerBehaviour.GetForCharater(other.gameObject);
		if (player != null && player.GetState() == PlayerBehaviour.State.ALIVE)
		{
			Application.LoadLevel (targetLevel);
		}
	}
}
