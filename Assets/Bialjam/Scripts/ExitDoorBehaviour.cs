using UnityEngine;
using System.Collections;

public class ExitDoorBehaviour : MonoBehaviour {

	public int targetLevel;

	void OnTriggerEnter(Collider other) {
		if (other.name == "Player") {
			Application.LoadLevel (targetLevel);
		}
	}
}
