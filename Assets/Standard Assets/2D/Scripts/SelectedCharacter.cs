using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
	[RequireComponent(typeof (PlatformerCharacter2D))]
	[RequireComponent(typeof (Camera2DFollow))]
	public class SelectedCharacter : MonoBehaviour
	{
		public GameObject currentCharacter;
		public Transform mainCamera;
		private PlatformerCharacter2D m_Character;
		private Camera2DFollow mainCameraFollow;
		private bool m_Jump;

		private void InitCurrentCharacter()
		{
			m_Character = currentCharacter.transform.GetComponent<PlatformerCharacter2D>();
			mainCameraFollow = mainCamera.GetComponent<Camera2DFollow>();
			mainCameraFollow.target = currentCharacter.transform;
		}

		public void SetCurrentCharacter(GameObject character)
		{
			currentCharacter = character;
			InitCurrentCharacter ();
		}
		
		private void Awake()
		{
			InitCurrentCharacter ();
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
			m_Character.Move(h, crouch, m_Jump);
			m_Jump = false;
		}
	}
}
