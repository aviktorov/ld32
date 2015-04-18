using UnityEngine;
using System.Collections;

/*
 */
public class HeroHands : MonoBehaviour {
	
	//
	public float throwStrength = 10.0f;
	public float throwBreakForce = 10.0f;
	
	//
	private Transform cachedTransform;
	private Rigidbody detectedObject;
	private Rigidbody grabbedObject;
	private FixedJoint grabbedJoint;
	
	//
	public Rigidbody GetGrabbedObject() {
		return grabbedObject;
	}
	
	public void Drop() {
		if(grabbedJoint) {
			Destroy(grabbedJoint);
			grabbedJoint = null;
		}
		
		grabbedObject = null;
	}
	
	//
	private void Awake() {
		cachedTransform = GetComponent<Transform>();
		
		grabbedObject = null;
		grabbedJoint = null;
		detectedObject = null;
	}
	
	//
	private void Update() {
		
		// grab
		if(Input.GetButtonDown("Grab") && detectedObject && !grabbedObject && !grabbedJoint) {
			grabbedObject = detectedObject;
			
			grabbedJoint = gameObject.AddComponent<FixedJoint>();
			grabbedJoint.connectedBody = grabbedObject;
			grabbedJoint.breakForce = throwBreakForce;
		}
		
		// stop holding if our joint was broken
		if(grabbedObject && !grabbedJoint) {
			grabbedObject = null;
		}
		
		// throw
		if(Input.GetButtonUp("Grab") && grabbedObject && grabbedJoint) {
			grabbedObject.AddForce(cachedTransform.right * throwStrength, ForceMode.Impulse);
			Drop();
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
