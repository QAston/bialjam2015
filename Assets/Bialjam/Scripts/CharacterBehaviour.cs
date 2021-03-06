using UnityEngine;
using System.Collections;

public class CharacterBehaviour : MonoBehaviour {

	private bool IsAlive = true;

	public enum Type
	{
		PLAYER,
		GHOST,
		NPC
	}

	public Type GetType() {
		if (gameObject.name == "PlayerCharacter")
			return Type.PLAYER;
		if (gameObject.tag == "GhostCharacter")
			return Type.GHOST;
		if (gameObject.tag == "NpcCharacter")
			return Type.NPC;
		throw new System.Exception ("invalid type");
	}
	
	[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
	[SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	private float m_MaxFallHeight = 3.0f;

	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
	private Animator m_Anim;            // Reference to the player's animator component.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private float lastGroundedPostion = 0f;
	public bool blockMovement = false;

	public void Die() {
		if (IsAlive) {
			IsAlive = false;
			m_Anim.SetInteger ("AliveState", 0);
			m_Rigidbody2D.velocity = new Vector2 (0, 0);
		}
	}

	public void Revive() {
		IsAlive = true;
		m_Anim.SetInteger ("AliveState", 2);
	}
	
	private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.FindChild("GroundCheck");
		m_CeilingCheck = transform.FindChild("CeilingCheck");
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		lastGroundedPostion = transform.position.y;
	}
	
	
	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
			}
		}

		if (m_Grounded) {
			if (Mathf.Abs(transform.position.y - lastGroundedPostion) > m_MaxFallHeight) {
				PlayerBehaviour p = PlayerBehaviour.GetForCharater(this.gameObject);
				// kill player if availabe, just play anim otherwise.
				if (p != null)
					p.DieCharacter ();
				else
					this.Die ();
			}
			lastGroundedPostion = transform.position.y;
		}

		m_Anim.SetBool("Ground", m_Grounded);
		
		// Set the vertical animationm_Grounded
		m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}

	
	public void OnCollisionEnter2D(Collision2D col) {
		// jako duch zderzamy się z npc
		if (GetType () == Type.NPC) {
			var npc = this;
			PlayerBehaviour p = PlayerBehaviour.GetForCharater(col.gameObject);
			if (p != null && npc.IsAlive)
			{
				var ghost = col.gameObject.GetComponent<CharacterBehaviour>();
				if (ghost != null && ghost.GetType() == Type.GHOST)
				{
					p.Possess(npc.gameObject);
					Destroy(ghost.gameObject);
				}
			}
		}
		// jako npc zderzamy się z graczem
		else if (GetType () == Type.PLAYER) {
			var player = this;
			PlayerBehaviour p = PlayerBehaviour.GetForCharater(col.gameObject);
			if (p != null)
			{
				var npc = col.gameObject.GetComponent<CharacterBehaviour>();
				if (npc != null && npc.GetType() == Type.NPC)
				{
					npc.GetComponent<Animator>().SetTrigger("Reanim");
					npc.blockMovement = true;
				}
			}
		}
	}

	public void StopVelocity() {
		m_Rigidbody2D.velocity = new Vector2 (0.0f, m_Rigidbody2D.velocity.y);
	}

	public void Fly(float vert, float hor) {
		m_Rigidbody2D.velocity = new Vector2(vert*m_MaxSpeed/2, hor*m_MaxSpeed/2);
	}

	public void Move(float move, bool crouch, bool jump) 
	{
		if (blockMovement)
			return;
		
		m_Grounded = true;

		// If crouching, check to see if the character can stand up
		if (!crouch && m_Anim.GetBool("Crouch"))
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}
		
		// Set whether or not the character is crouching in the animator
		m_Anim.SetBool("Crouch", crouch);
		
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Reduce the speed if crouching by the crouchSpeed multiplier
			move = (crouch ? move*m_CrouchSpeed : move);
			
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat("Speed", Mathf.Abs(move));
			
			// Move the character
			m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);
			
			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (m_Grounded && jump && m_Anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Anim.SetBool ("Ground", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}


	}
	
	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}

