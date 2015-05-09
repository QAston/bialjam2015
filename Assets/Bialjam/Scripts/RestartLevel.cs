using UnityEngine;
using System.Collections;

public class RestartLevel : MonoBehaviour {
	public void RestartCurrentLevel() {
		Application.LoadLevel (Application.loadedLevel);
	}
}
