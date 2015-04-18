using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JamSuite.Events;

/*
 */
public class HeroDamageHandler : MonoBehaviour {
	
	//
	public float damageVelocityThreshold = 10.0f;
	public float landingTime = 2.0f;
	public FloatEvent onDamage = null;
	public UnityEvent onLand = null;
	
	//
	private float currentLandingTime;
	private bool checkLanding;
	
	//
	private void Awake() {
		currentLandingTime = 0.0f;
		checkLanding = false;
	}
	
	//
	private void OnCollisionStay(Collision collision) {
		if(!checkLanding) return;
		
		currentLandingTime -= Time.fixedDeltaTime;
		if(currentLandingTime < 0.0f) {
			onLand.Invoke();
			checkLanding = false;
		}
	}
	
	private void OnCollisionExit(Collision collision) {
		currentLandingTime = landingTime;
	}
	
	private void OnCollisionEnter(Collision collision) {
		checkLanding = true;
		currentLandingTime = landingTime;
		
		float magnitude = collision.relativeVelocity.magnitude;
		if(magnitude < damageVelocityThreshold) return;
		
		onDamage.Invoke(magnitude);
	}
}
