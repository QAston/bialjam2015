using UnityEngine;
using System.Collections;

public class BottomLimiter : MonoBehaviour {
	private float minY;

	// Use this for initialization
	void Start () {
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		foreach (Renderer r in FindObjectsOfType(typeof(Renderer)))
		        b.Encapsulate(r.bounds);
		minY = b.min.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position.y < minY) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
