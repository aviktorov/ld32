using UnityEngine;
using System.Collections;

/*
 */
public class HeroHands : MonoBehaviour {
	
	//
	private Transform cachedTransform;
	private GameObject cachedHero;
	
	private Rigidbody detectedObject;
	private Rigidbody grabbedObject;
	private FixedJoint grabbedJoint;
	
	//
	public void SetHero(GameObject hero) {
		cachedHero = hero;
	}
	
	public Rigidbody GetGrabbedObject() {
		return grabbedObject;
	}
	
	public void Drop(float throwStrength = 0.0f) {
		if(!grabbedObject) return;
		if(!grabbedJoint) return;
		
		Destroy(grabbedJoint);
		grabbedJoint = null;
		
		if(throwStrength > 0.0f) grabbedObject.AddForce(cachedTransform.right * throwStrength,ForceMode.Impulse);
		grabbedObject = null;
	}
	
	public void Grab(float breakForce = Mathf.Infinity) {
		if(detectedObject == null) return;
		if(grabbedObject) return;
		if(grabbedJoint) return;
		
		grabbedObject = detectedObject;
		
		grabbedJoint = gameObject.AddComponent<FixedJoint>();
		grabbedJoint.connectedBody = grabbedObject;
		grabbedJoint.breakForce = breakForce;
		
		grabbedObject.gameObject.BroadcastMessage("PrepareForGrab",cachedHero,SendMessageOptions.DontRequireReceiver);
	}
	
	//
	private void Awake() {
		cachedTransform = GetComponent<Transform>();
		cachedHero = null;
		
		grabbedObject = null;
		grabbedJoint = null;
		detectedObject = null;
	}
	
	//
	private void Update() {
		
		// stop holding if our joint was broken
		if(grabbedObject && !grabbedJoint) {
			grabbedObject = null;
		}
	}
	
	//
	private void OnTriggerEnter(Collider collider) {
		Rigidbody body = collider.attachedRigidbody;
		if(body == null) return;
		if(body.isKinematic) return;
		
		detectedObject = body;
	}
	
	private void OnTriggerExit(Collider collider) {
		detectedObject = null;
	}
}
