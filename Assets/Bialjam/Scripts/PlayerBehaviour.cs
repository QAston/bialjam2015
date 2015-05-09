using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerBehaviour : MonoBehaviour {

	enum State {
		ALIVE,
		GHOST,
		POSSESSING,
	}

	private State currentState;

	public GameObject startCharacter;

	public void DieCharacter() {

	}

	public void Possess(GameObject character){
		possessedCharacter = character;
	}

	void Restart() {
	}
	
	private GameObject possessedCharacter;
	private CharacterBehaviour possessedCharacterBehavior;
	private bool m_Jump;

	public GameObject GetCharacter() {
		return possessedCharacter;
	}

	void Start() {
		currentState = State.ALIVE;
		possessedCharacter = startCharacter;
	}

	public void Revive() {
		currentState = State.ALIVE;
	}
	
	private void Awake()
	{
		possessedCharacterBehavior = possessedCharacter.GetComponent<CharacterBehaviour>();
	}
	
	
	private void Update()
	{
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed.
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}
	
	
	private void FixedUpdate()
	{
		// Read the inputs.
		bool crouch = Input.GetKey(KeyCode.LeftControl);
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		// Pass all parameters to the character control script.
		possessedCharacterBehavior.Move(h, crouch, m_Jump);
		m_Jump = false;
		transform.position = possessedCharacter.transform.position;
	}
}

