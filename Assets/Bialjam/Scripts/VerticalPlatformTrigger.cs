using UnityEngine;
using System.Collections;

public class VerticalPlatformTrigger : MonoBehaviour {

	public GameObject obj;
	bool trigger = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter2D(Collision2D col) {
		if(!trigger){
			obj.GetComponent<Animator>().Play ("PlatformVerticalAnimation");
			trigger = !trigger;
		}else{
			obj.GetComponent<Animator>().speed = 1;
		}

	}

	void OnCollisionExit2D(Collision2D col){
		obj.GetComponent<Animator>().speed = 0;
	}
}
