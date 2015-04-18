using UnityEngine;
using System.Collections;
using JamSuite.Events;

/*
 */
public class HeroDamageHandler : MonoBehaviour {
	
	//
	public float damageVelocityTreshold = 10.0f;
	public FloatEvent onDamage = null;
	
	//
	private void OnCollisionEnter(Collision collision) {
		float magnitude = collision.relativeVelocity.magnitude;
		if(magnitude < damageVelocityTreshold) return;
		
		onDamage.Invoke(magnitude);
	}
}
