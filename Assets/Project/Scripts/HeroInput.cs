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
	public string jump = "Jump";
	
	[Header("Physics")]
	public float maxVelocity = 10.0f;
	public float acceleration = 10.0f;
	public float jumpVelocity = 2.0f;
	
	public float throwStrength = 10.0f;
	public float throwBreakForce = 10.0f;
	
	//
	private Rigidbody cachedBody;
	private HeroHands cachedHands;
	private HeroCollisions cachedCollision;
	private Transform cachedCameraTransform;
	private Transform cachedTransform;
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
		cachedHands = GetComponent<HeroHands>();
		cachedTransform = GetComponent<Transform>();
		cachedCollision = GetComponent<HeroCollisions>();
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
		
		// orient
		if(input.sqrMagnitude > 0.0f) cachedTransform.rotation = Quaternion.LookRotation(new Vector3(-movement.z,0.0f,movement.x).normalized,Vector3.up);
		
		// jump
		if(Input.GetButtonDown(jump) && !cachedCollision.InAir()) {
			cachedBody.velocity = cachedBody.velocity.WithY(jumpVelocity);
			cachedCollision.SetInAir(true);
		}
		
		// grab
		if(Input.GetButtonDown(grab)) cachedHands.Grab(throwBreakForce);
		if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength);
	}
}
