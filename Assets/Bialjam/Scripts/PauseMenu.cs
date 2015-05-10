using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public Canvas pauseMenu;
	private bool showMenu = false;

	// Use this for initialization
	void Start () {
		pauseMenu.enabled = false;
		showMenu = false;
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyUp (KeyCode.R)) {
			Application.LoadLevel(Application.loadedLevel);
		}
		if (Input.GetKeyUp (KeyCode.Escape)) {
			showMenu = !showMenu;
		}
	}

	void OnGUI() {
		if (showMenu) {
			pauseMenu.enabled = true;
			Time.timeScale = 0.0f;
			Screen.lockCursor = false;
			Cursor.visible = true;
		} else {
			pauseMenu.enabled = false;
			Time.timeScale = 1.0f;
			Screen.lockCursor = true;
			Cursor.visible = false;
		}
	}
}
