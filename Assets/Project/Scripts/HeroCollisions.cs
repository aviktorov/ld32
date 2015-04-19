using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JamSuite.Events;

/*
 */
public class HeroCollisions : MonoBehaviour {
	
	//
	public float groundTreshold = 0.7f;
	public float damageVelocityThreshold = 10.0f;
	public float landingTime = 2.0f;
	public FloatEvent onDamage = null;
	public UnityEvent onLand = null;
	
	//
	private float currentLandingTime;
	private bool checkLanding;
	private bool inAir;
	
	//
	public bool InAir() { return inAir; }
	public void SetInAir(bool air) { inAir = air; }
	
	public void SetCheckLanding(bool landing) { checkLanding = landing; }
	
	//
	private void Awake() {
		currentLandingTime = 0.0f;
		checkLanding = false;
		inAir = true;
	}
	
	//
	private void OnCollisionStay2D(Collision2D collision) {
		if(!checkLanding) return;
		
		currentLandingTime -= Time.fixedDeltaTime;
		if(currentLandingTime < 0.0f) {
			onLand.Invoke();
			checkLanding = false;
		}
	}
	
	private void OnCollisionExit2D(Collision2D collision) {
		currentLandingTime = landingTime;
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
		Vector2 averageNormal = Vector2.zero;
		foreach(ContactPoint2D point in collision.contacts) {
			averageNormal += point.normal;
		}
		
		averageNormal.Normalize();
		if(averageNormal.y > groundTreshold) inAir = false;
		
		currentLandingTime = landingTime;
		
		float magnitude = collision.relativeVelocity.magnitude;
		if(magnitude < damageVelocityThreshold) return;
		
		onDamage.Invoke(magnitude);
	}
}
