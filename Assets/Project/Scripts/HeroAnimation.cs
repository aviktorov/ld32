using UnityEngine;
using System.Collections;

/*
 */
public class HeroAnimation : MonoBehaviour {
	
	//
	private Animator cachedAnimation;
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroStats cachedHero;
	private HeroCollisions cachedCollision;
	private HeroInput cachedInput;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animator>();
		cachedBody = GetComponent<Rigidbody2D>();
		cachedHands = GetComponent<HeroHands>();
		cachedHero = GetComponent<HeroStats>();
		cachedInput = GetComponent<HeroInput>();
		cachedCollision = GetComponent<HeroCollisions>();
	}
	
	private void Update () {
		Vector3 velocity = cachedBody.velocity;
		cachedAnimation.SetFloat("VelocityY",velocity.y);
		cachedAnimation.SetFloat("Grabbing",cachedHands.GetGrabbedObject() ? 1.0f : 0.0f);
		
		cachedAnimation.SetBool("InAir",cachedCollision.InAir());
		cachedAnimation.SetBool("Moving",cachedInput.IsMoving());
		cachedAnimation.SetBool("Stunned",cachedHero.IsStunned());
	}
}
