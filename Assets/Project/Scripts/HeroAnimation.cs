using UnityEngine;
using System.Collections;

/*
 */
public class HeroAnimation : MonoBehaviour {
	
	//
	private Animator cachedAnimation;
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroCollisions cachedCollision;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animator>();
		cachedBody = GetComponent<Rigidbody2D>();
		cachedHands = GetComponent<HeroHands>();
		cachedCollision = GetComponent<HeroCollisions>();
	}
	
	private void Update () {
		Vector3 velocity = cachedBody.velocity;
		cachedAnimation.SetFloat("VelocityX",velocity.x);
		cachedAnimation.SetFloat("VelocityY",velocity.y);
		
		cachedAnimation.SetBool("Grabbing",cachedHands.GetGrabbedObject() != null);
		cachedAnimation.SetBool("InAir",cachedCollision.InAir());
	}
}
