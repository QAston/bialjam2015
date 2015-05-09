using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RandomEnable : MonoBehaviour {
	private List<GameObject> objects;
	public float switchTime = 5.0f;
	private int currentIndex;
	private float counter;

	void Awake () {
		objects = new List<GameObject> ();
		foreach (Transform child in transform) {
			objects.Add(child.gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		counter = switchTime;
		foreach (GameObject obj in objects) {
			obj.SetActive(false);
		}
		currentIndex = Random.Range (0, objects.Count);
		objects[currentIndex].SetActive (true);
	}
	
	// Update is called once per frame
	void Update () {
		counter -= Time.deltaTime;
		if (counter < 0.0f) {
			objects[currentIndex].SetActive(false);
			currentIndex = Random.Range (0, objects.Count);
			objects[currentIndex].SetActive(true);
			counter = switchTime;
		}
	}
}
