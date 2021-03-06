﻿using UnityEngine;
using System.Collections;

/*
 */
public class HeroHands : MonoBehaviour {
	
	//
	public float grabMass = 1.0f;
	public float grabOffset = 1.5f;
	public float waitPhysics = 1.0f;
	
	//
	private Rigidbody2D cachedBody;
	private Transform cachedTransform;
	
	private Rigidbody2D detectedObject;
	private Rigidbody2D grabbedObject;
	private HingeJoint2D grabbedJoint;
	private float grabbedMass;
	
	//
	public Rigidbody2D GetGrabbedObject() { return grabbedObject; }
	
	public void Drop(float throwStrength = 0.0f,float throwVertical = 0.0f) {
		if(!grabbedObject) return;
		
		if(grabbedJoint) {
			Destroy(grabbedJoint);
			grabbedJoint = null;
		}
		
		grabbedObject.velocity = Vector2.zero;
		grabbedObject.angularVelocity = 0.0f;
		
		StartCoroutine(WaitAndRestorePhysics(grabbedObject.gameObject,cachedBody.gameObject));
		
		if(throwStrength > 0.0f) {
			Vector2 throwDirection = new Vector2(cachedTransform.localScale.x,throwVertical);
			grabbedObject.AddForce(throwDirection.normalized * throwStrength,ForceMode2D.Impulse);
		}
		
		grabbedObject.gameObject.BroadcastMessage("PrepareForDrop",throwStrength > 0.0f,SendMessageOptions.DontRequireReceiver);
		
		grabbedObject.mass = grabbedMass;
		grabbedObject = null;
	}
	
	public void Grab() {
		if(detectedObject == null) return;
		if(grabbedObject) return;
		if(grabbedJoint) return;
		
		grabbedObject = detectedObject;
		
		HeroInput grabbedInput = grabbedObject.GetComponent<HeroInput>();
		if(grabbedInput && grabbedInput.IsBlocking()) {
			grabbedObject = null;
			return;
		}
		
		JamSuite.Physics2D.IgnoreCollision(grabbedObject.gameObject,cachedBody.gameObject,true);
		
		grabbedObject.transform.position = cachedTransform.position + Vector3.up * grabOffset;
		grabbedObject.gameObject.BroadcastMessage("PrepareForGrab",SendMessageOptions.DontRequireReceiver);
		
		grabbedMass = grabbedObject.mass;
		grabbedObject.mass = grabMass;
		
		grabbedJoint = gameObject.AddComponent<HingeJoint2D>();
		grabbedJoint.anchor = new Vector2(0.0f,grabOffset);
		grabbedJoint.connectedBody = grabbedObject;
		grabbedJoint.useLimits = true;
		grabbedJoint.limits = new JointAngleLimits2D() { min = 0.0f, max = 0.0f };
	}
	
	
	//
	private IEnumerator WaitAndRestorePhysics(GameObject first,GameObject second) {
		yield return new WaitForSeconds(waitPhysics);
		JamSuite.Physics2D.IgnoreCollision(first,second,false);
	}
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody2D>();
		cachedTransform = GetComponent<Transform>();
		
		grabbedObject = null;
		grabbedJoint = null;
		detectedObject = null;
	}
	
	//
	private void OnTriggerEnter2D(Collider2D collider) {
		Rigidbody2D body = collider.attachedRigidbody;
		if(body == null) return;
		if(body.isKinematic) return;
		
		detectedObject = body;
	}
	
	private void OnTriggerExit2D(Collider2D collider) {
		detectedObject = null;
	}
}
