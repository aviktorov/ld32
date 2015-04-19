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
	
	//
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroCollisions cachedCollision;
	private Transform cachedTransform;
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody2D>();
		cachedHands = GetComponent<HeroHands>();
		cachedTransform = GetComponent<Transform>();
		cachedCollision = GetComponent<HeroCollisions>();
	}
	
	private void Start() {
		if(!Camera.main) Debug.LogError("No main camera on scene");
		if(!cachedHands) Debug.LogError("Handless hero, yikes");
	}
	
	//
	private void Update() {
		if(cachedBody == null) return;
		if(cachedHands == null) return;
		
		// movement
		Vector2 input = new Vector2(Input.GetAxis(moveHorizontal),0.0f);
		
		Vector2 movement = input;
		if(movement.sqrMagnitude > 1.0f) movement.Normalize();
		
		Vector2 newVelocity = cachedBody.velocity + movement * acceleration * Time.deltaTime;
		newVelocity.x = Mathf.Clamp(newVelocity.x,-maxVelocity,maxVelocity);
		
		cachedBody.velocity = newVelocity;
		
		// orient
		if(input.sqrMagnitude > 0.0f) cachedTransform.localScale = new Vector3(Mathf.Sign(input.x),1.0f,1.0f);
		
		// jump
		if(Input.GetButtonDown(jump) && !cachedCollision.InAir()) {
			cachedBody.velocity = cachedBody.velocity.WithY(jumpVelocity);
			cachedCollision.SetInAir(true);
		}
		
		// grab
		if(Input.GetButtonDown(grab)) cachedHands.Grab();
		if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength);
	}
}
