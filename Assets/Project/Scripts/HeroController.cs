using UnityEngine;
using System.Collections;

/*
 */
public class HeroController : MonoBehaviour {
	
	//
	public float maxVelocity = 10.0f;
	public float acceleration = 10.0f;
	
	public Transform handsTransform = null;
	
	//
	private Rigidbody cachedBody;
	private Transform cachedCameraTransform;
	
	//
	private Vector2 GetInput(string horizontal,string vertical) {
		Vector2 input = new Vector2(Input.GetAxis(horizontal),Input.GetAxis(vertical));
		return input;
	}
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
	}
	
	private void Start() {
		cachedCameraTransform = Camera.main.GetComponent<Transform>();
	}
	
	private void Update() {
		if(cachedBody == null) return;
		
		Vector2 hands = GetInput("HandsHorizontal","HandsVertical").normalized;
		Vector2 legs = GetInput("LegsHorizontal","LegsVertical");
		
		Vector3 movement = cachedCameraTransform.right * legs.x + cachedCameraTransform.forward * legs.y;
		if(movement.sqrMagnitude > 1.0f) movement.Normalize();
		
		float angle = Mathf.Atan2(hands.y,hands.x) * Mathf.Rad2Deg;
		Debug.Log(string.Format("Hands: {0}, Angle: {1}",hands,angle));
		
		handsTransform.eulerAngles = handsTransform.eulerAngles.WithZ(angle);
		
		Vector3 newVelocity = cachedBody.velocity + movement * acceleration * Time.deltaTime;
		newVelocity.y = 0.0f;
		
		if(newVelocity.sqrMagnitude > maxVelocity * maxVelocity) newVelocity = newVelocity.normalized * maxVelocity;
		cachedBody.velocity = newVelocity.WithY(cachedBody.velocity.y);
	}
}
