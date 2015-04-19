using UnityEngine;
using System.Collections;

/*
 */
public class HeroHands : MonoBehaviour {
	
	//
	public float grabMass = 1.0f;
	public float grabOffset = 1.5f;
	
	//
	private Rigidbody cachedBody;
	private Transform cachedTransform;
	
	private Rigidbody detectedObject;
	private Rigidbody grabbedObject;
	private float grabbedMass;
	private FixedJoint grabbedJoint;
	
	//
	public Rigidbody GetGrabbedObject() { return grabbedObject; }
	
	public void Drop(float throwStrength = 0.0f) {
		if(!grabbedObject) return;
		
		if(grabbedJoint) {
			Destroy(grabbedJoint);
			grabbedJoint = null;
		}
		
		
		grabbedObject.velocity = Vector3.zero;
		grabbedObject.angularVelocity = Vector3.zero;
		
		JamSuite.Physics.IgnoreCollision(grabbedObject,cachedBody,false);
		if(throwStrength > 0.0f) grabbedObject.AddForce(cachedTransform.right * throwStrength,ForceMode.Impulse);
		
		grabbedObject.mass = grabbedMass;
		grabbedObject = null;
	}
	
	public void Grab(float breakForce = Mathf.Infinity) {
		if(detectedObject == null) return;
		if(grabbedObject) return;
		if(grabbedJoint) return;
		
		grabbedObject = detectedObject;
		JamSuite.Physics.IgnoreCollision(grabbedObject,cachedBody,true);
		
		grabbedObject.transform.position = cachedTransform.position + Vector3.up * grabOffset;
		grabbedObject.gameObject.BroadcastMessage("PrepareForGrab",SendMessageOptions.DontRequireReceiver);
		
		grabbedMass = grabbedObject.mass;
		grabbedObject.mass = grabMass;
		
		grabbedJoint = gameObject.AddComponent<FixedJoint>();
		grabbedJoint.breakForce = breakForce;
		grabbedJoint.connectedBody = grabbedObject;
	}
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
		cachedTransform = GetComponent<Transform>();
		
		grabbedObject = null;
		grabbedJoint = null;
		detectedObject = null;
	}
	
	//
	private void Update() {
		
		// stop holding if our joint was broken
		if(grabbedObject && !grabbedJoint) {
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
