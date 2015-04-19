using UnityEngine;
using System.Collections;

/*
 */
public class HeroAnimation : MonoBehaviour {
	
	//
	private Animator cachedAnimation;
	private Rigidbody cachedBody;
	private HeroHands cachedHands;
	private HeroDamageHandler cachedCollision;
	
	private void Awake() {
		cachedAnimation = GetComponent<Animator>();
		cachedBody = GetComponent<Rigidbody>();
		cachedHands = GetComponent<HeroHands>();
		cachedCollision = GetComponent<HeroDamageHandler>();
	}
	
	private void Update () {
		Vector3 velocity = cachedBody.velocity;
		cachedAnimation.SetFloat("VelocityX",velocity.x);
		cachedAnimation.SetFloat("VelocityY",velocity.y);
		
		cachedAnimation.SetBool("Grabbng",cachedHands.GetGrabbedObject() != null);
		cachedAnimation.SetBool("InAir",cachedCollision.InAir());
	}
}
