using UnityEngine;
using System.Collections;

/*
 */
public class HeroInput : MonoBehaviour {
	
	//
	[Header("Input")]
	public string moveHorizontal = "Horizontal";
	public string moveVertical = "Vertical";
	
	public string grab = "Grab";
	
	[Header("Physics")]
	public float maxVelocity = 10.0f;
	public float acceleration = 10.0f;
	
	public float throwStrength = 10.0f;
	public float throwBreakForce = 10.0f;
	
	//
	private Rigidbody cachedBody;
	private Transform cachedCameraTransform;
	private HeroHands cachedHands;
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
		cachedHands = GetComponent<HeroHands>();
	}
	
	private void Start() {
		if(!Camera.main) Debug.LogError("No main camera on scene");
		if(!cachedHands) Debug.LogError("Handless hero, yikes");
		
		cachedCameraTransform = Camera.main.GetComponent<Transform>();
	}
	
	//
	private void Update() {
		if(cachedBody == null) return;
		if(cachedHands == null) return;
		
		// movement
		Vector2 input = new Vector2(Input.GetAxis(moveHorizontal),Input.GetAxis(moveVertical));
		
		Vector3 movement = cachedCameraTransform.right * input.x + cachedCameraTransform.forward * input.y;
		if(movement.sqrMagnitude > 1.0f) movement.Normalize();
		
		Vector3 newVelocity = cachedBody.velocity + movement * acceleration * Time.deltaTime;
		newVelocity.y = 0.0f;
		
		if(newVelocity.sqrMagnitude > maxVelocity * maxVelocity) newVelocity = newVelocity.normalized * maxVelocity;
		cachedBody.velocity = newVelocity.WithY(cachedBody.velocity.y);
		
		// orient?
		
		// grab
		if(Input.GetButtonDown(grab)) cachedHands.Grab(throwBreakForce);
		if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength);
	}
}
