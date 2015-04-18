using UnityEngine;
using System.Collections;

/*
 */
public class HeroController : MonoBehaviour {
	
	//
	public float maxVelocity = 10.0f;
	public float acceleration = 10.0f;
	
	//
	private Rigidbody cachedBody;
	
	//
	private Vector3 GetDirection(string horizontal, string vertical) {
		Vector3 direction = Vector3.right * Input.GetAxis(horizontal) + Vector3.forward * Input.GetAxis(vertical);
		if(direction.sqrMagnitude > 1.0f) direction.Normalize();
		
		return direction;
	}
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
	}
	
	private void Update() {
		if(cachedBody == null) return;
		
		Vector3 movement = GetDirection("LegsHorizontal","LegsVertical");
		Vector3 hands = GetDirection("HandsHorizontal","HandsVertical");
		
		Vector3 newVelocity = cachedBody.velocity + movement * acceleration * Time.deltaTime;
		newVelocity.y = 0.0f;
		
		if(newVelocity.sqrMagnitude > maxVelocity * maxVelocity) newVelocity = newVelocity.normalized * maxVelocity;
		cachedBody.velocity = newVelocity.WithY(cachedBody.velocity.y);
	}
}
