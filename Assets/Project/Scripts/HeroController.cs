using UnityEngine;
using System.Collections;

/*
 */
public class HeroController : MonoBehaviour {
	
	//
	public float velocity = 10.0f;
	
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
		
		cachedBody.velocity = cachedBody.velocity + movement * velocity * Time.deltaTime;
	}
}
