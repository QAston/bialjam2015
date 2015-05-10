using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerBehaviour : MonoBehaviour {

	public enum State {
		ALIVE,
		GHOST,
		POSSESSING,
	}

	public State GetState() {
		return currentState;
	}

	private State currentState;

	public GameObject startCharacter;
	public GameObject ghostToSpawn;

	public void DieCharacter() {
		if (currentState != State.GHOST) {
			currentState = State.GHOST;
			GameObject ghost = (GameObject)Instantiate(ghostToSpawn, transform.position, transform.rotation);
			possessedCharacterBehavior.Die();
			initPossess(ghost);
		}
	}

	public void Possess(GameObject character){
		currentState = State.POSSESSING;
		initPossess(character);
	}

	void Restart() {
	}

	private void initPossess(GameObject character)
	{
		if (possessedCharacter != null) {
			possessedCharacter.GetComponent<SpriteRenderer> ().sortingOrder = 1;
			possessedCharacter.transform.SetParent (null);
		}
		character.GetComponent<SpriteRenderer> ().sortingOrder = 2; // bring to front
		possessedCharacter = character;
		possessedCharacterBehavior = possessedCharacter.GetComponent<CharacterBehaviour>();
		possessedCharacter.transform.SetParent(this.gameObject.transform.parent);

		var camera = GameObject.Find ("PlayerCamera");
		var ghostMask = LayerMask.GetMask ("Ghost Level", "Ghost Background", "Ghost Player");
		var aliveMask = LayerMask.GetMask ("Alive Level", "Alive Background");
		if (camera != null) {

			var camComponent = camera.GetComponent<Camera>();
			switch (possessedCharacterBehavior.GetType ()) {
			case CharacterBehaviour.Type.PLAYER:
			case CharacterBehaviour.Type.NPC:
				camComponent.cullingMask |= aliveMask;
				camComponent.cullingMask &= ~ghostMask;
				break;
			case CharacterBehaviour.Type.GHOST:
				camComponent.cullingMask |= ghostMask;
				camComponent.cullingMask &= ~aliveMask;
				break;
			}

		}
	}
	
	private GameObject possessedCharacter;
	private CharacterBehaviour possessedCharacterBehavior;
	private bool m_Jump;

	public GameObject GetCharacter() {
		return possessedCharacter;
	}

	private void Start() {
		currentState = State.ALIVE;
		initPossess(startCharacter);
		// init min position
		bounds = new Bounds(Vector3.zero, Vector3.zero);
		foreach (Renderer r in FindObjectsOfType(typeof(Renderer)))
			bounds.Encapsulate(r.bounds);
	}
	
	public void Revive(GameObject player) {
		currentState = State.ALIVE;
		initPossess(player);
	}
	
	private void Awake()
	{
		if (possessedCharacter == null) {
			Start();
		}
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
		if (GetState () != State.GHOST) {
			// Read the inputs.
			bool crouch = Input.GetKey (KeyCode.LeftControl);
			float h = CrossPlatformInputManager.GetAxis ("Horizontal");

			// Pass all parameters to the character control script.
			possessedCharacterBehavior.Move (h, crouch, m_Jump);
		} else {
			possessedCharacterBehavior.Fly(CrossPlatformInputManager.GetAxis ("Horizontal"), CrossPlatformInputManager.GetAxis ("Vertical"));
		}
		m_Jump = false;
		transform.position = possessedCharacter.transform.position;

		// check if within map
		if (!bounds.Contains(transform.position)) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}

	public static PlayerBehaviour GetForCharater (GameObject character) {
		return (character != null && character.transform.parent != null) ? character.transform.parent.GetComponentInChildren<PlayerBehaviour>() : null;
	}

	private Bounds bounds;
}

