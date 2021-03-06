﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Movement variables
	public float maxSpeed;

	// Jumping variables
	bool grounded = false;
	float groundCheckRadius = 0.2f;
	public LayerMask groundLayer;
	public Transform groundCheck;
	public float jumpHeight;

	Rigidbody2D myRB;
	Animator myAnim;
	bool facingRight;

	// For shooting
	public Transform weaponMuzzle;
	public GameObject bullet;
	float fireRate = 0.5f;
	float nextFire = 0f;

	// Use this for initialization
	void Start () {
		myRB = GetComponent<Rigidbody2D> ();
		myAnim = GetComponent<Animator> ();

		facingRight = true;
	}

	void Update() {
		if (grounded && Input.GetAxis ("Jump")>0) {
			grounded = false;
			myAnim.SetBool ("isGrounded", grounded);
			myRB.AddForce (new Vector2 (0, jumpHeight));
		}

		// Player shooting
		if (Input.GetAxisRaw ("Fire1") > 0) {
			fire ();
		}
	}

	// Update is called every period of time
	void FixedUpdate () {

		// Check if we are grounded - if no, then we are falling
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
		myAnim.SetBool ("isGrounded", grounded);

		myAnim.SetFloat ("verticalSpeed", myRB.velocity.y);

		float move = Input.GetAxis ("Horizontal");
		myAnim.SetFloat ("speed", Mathf.Abs(move));

		// Move the character
		myRB.velocity = new Vector2 (move * maxSpeed, myRB.velocity.y);

		// If we move right & the character is not facing right, flip
		if(move>0 && !facingRight){
			flip ();
		// And if we move left & the character is facing right, flip
		} else if(move<0 && facingRight) {
			flip ();
		}
	}

	void flip(){
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1; // Same as: theScale.x = theScale.x * -1
		transform.localScale = theScale;
	}

	void fire(){
		if (Time.time > nextFire) {
			nextFire = Time.time + fireRate;
			if (facingRight) {
				Instantiate (bullet, weaponMuzzle.position, Quaternion.Euler (new Vector3 (0, 0, 0)));
			} else if(!facingRight) {
				Instantiate (bullet, weaponMuzzle.position, Quaternion.Euler (new Vector3 (0, 0, 180f)));
			}
		}
	}
}
