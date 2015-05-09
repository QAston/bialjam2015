using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof (Text))]
public class CurrentLevel : MonoBehaviour {
	Text currentLevelText;

	// Use this for initialization
	void Start () {
		currentLevelText = GetComponent<Text> ();
		currentLevelText.text = "Level " + Application.loadedLevelName;
	}
}
