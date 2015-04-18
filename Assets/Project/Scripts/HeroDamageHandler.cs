using UnityEngine;
using System.Collections;

/*
 */
public class HeroDamageHandler : MonoBehaviour {
	
	//
	public float damageVelocityTreshold = 10.0f;
	
	//
	private void OnCollisionEnter(Collision collision) {
		if(collision.relativeVelocity.magnitude < damageVelocityTreshold) return;
		
		Debug.Log("Ouch");
	}
}
