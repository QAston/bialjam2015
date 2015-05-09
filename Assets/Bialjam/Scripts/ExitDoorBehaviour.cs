using UnityEngine;
using System.Collections;

public class ExitDoorBehaviour : MonoBehaviour {

	public int targetLevel;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			Application.LoadLevel (targetLevel);
		}
	}
}
