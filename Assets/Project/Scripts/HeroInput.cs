using UnityEngine;
using System.Collections;

/*
 */
public class HeroInput : MonoBehaviour {
	
	//
	[Header("Input")]
	public string legsHorizontal = "LegsHorizontal";
	public string legsVertical = "LegsVertical";
	
	public string handsHorizontal = "HandsHorizontal";
	public string handsVertical = "HandsVertical";
	
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
	private Transform cachedHandsTransform;
	
	//
	private Vector2 GetInput(string horizontal,string vertical) {
		return new Vector2(Input.GetAxis(horizontal),Input.GetAxis(vertical));
	}
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody>();
		cachedHands = GetComponentInChildren<HeroHands>();
	}
	
	private void Start() {
		if(!Camera.main) Debug.LogError("No main camera on scene");
		if(!cachedHands) Debug.LogError("Handless hero, yikes");
		
		cachedCameraTransform = Camera.main.GetComponent<Transform>();
		cachedHandsTransform = cachedHands.transform;
		
		Collider collider = GetComponent<Collider>();
		Collider[] hands = cachedHands.GetComponentsInChildren<Collider>();
		
		foreach(Collider hand in hands) {
			Physics.IgnoreCollision(collider,hand);
		}
	}
	
	//
	private void Update() {
		if(cachedBody == null) return;
		if(cachedHands == null) return;
		if(cachedCameraTransform == null) return;
		
		// legs & hands
		Vector2 hands = GetInput(handsHorizontal,handsVertical).normalized;
		Vector2 legs = GetInput(legsHorizontal,legsVertical);
		
		Vector3 movement = cachedCameraTransform.right * legs.x + cachedCameraTransform.forward * legs.y;
		if(movement.sqrMagnitude > 1.0f) movement.Normalize();
		
		float angle = Mathf.Atan2(hands.y,hands.x) * Mathf.Rad2Deg;
		cachedHandsTransform.eulerAngles = cachedHandsTransform.eulerAngles.WithZ(angle);
		
		Vector3 newVelocity = cachedBody.velocity + movement * acceleration * Time.deltaTime;
		newVelocity.y = 0.0f;
		
		if(newVelocity.sqrMagnitude > maxVelocity * maxVelocity) newVelocity = newVelocity.normalized * maxVelocity;
		cachedBody.velocity = newVelocity.WithY(cachedBody.velocity.y);
		
		// grab
		if(Input.GetButtonDown(grab)) cachedHands.Grab(throwBreakForce);
		if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength);
	}
}
