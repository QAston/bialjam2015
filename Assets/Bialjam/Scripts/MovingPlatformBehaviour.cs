using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class MovingPlatformBehaviour : MonoBehaviour {
	
	private Transform onPlatform = null;
	private Vector3 lastPos;

	// Use this for initialization
	void Start() {

	}

	void OnTriggerEnter2D(Collider2D other) {
		var player = other.GetComponent<PlatformerCharacter2D> ();
		if (player != null) {
			onPlatform = other.transform;
			return;
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		var player = other.GetComponent<PlatformerCharacter2D> ();
		if (player != null) {
			onPlatform = null;
			return;
		}
	}

	void Update ()
	{
		lastPos = transform.position;
	}

	void LateUpdate() {
		if (onPlatform)
			onPlatform.position += (transform.position - lastPos);
	}
}