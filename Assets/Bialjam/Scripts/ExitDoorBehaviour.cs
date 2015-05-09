using UnityEngine;
using System.Collections;

public class ExitDoorBehaviour : MonoBehaviour {

	public int targetLevel;

	void OnTriggerEnter2D(Collision2D other) {
		var player = PlayerBehaviour.GetForCharater(other.gameObject);
		if (player != null && player.GetState() == PlayerBehaviour.State.ALIVE)
		{
			Application.LoadLevel (targetLevel);
		}
	}
}
