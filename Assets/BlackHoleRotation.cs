using UnityEngine;
using System.Collections;

public class BlackHoleRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0, 0, 4 * Time.deltaTime);
	}
}
