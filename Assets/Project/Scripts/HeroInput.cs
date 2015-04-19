using UnityEngine;
using System.Collections;

/*
 */
public class HeroInput : MonoBehaviour {
	
	//
	[Header("Input")]
	public string moveHorizontal = "Horizontal";
	
	public string grab = "Grab";
	public string jump = "Jump";
	public string block = "Block";
	
	[Header("Physics")]
	public float moveVelocity = 10.0f;
	public float jumpVelocity = 2.0f;
	public int maxJumps = 2;
	
	public float throwStrength = 10.0f;
	public float throwVertical = 0.2f;
	
	//
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroStats cachedHero;
	private HeroCollisions cachedCollision;
	private Transform cachedTransform;
	
	private bool isMoving;
	private bool isGrabbed;
	private bool isBlocking;
	private int currentJumps;
	
	//
	public bool IsMoving() { return isMoving; }
	public bool IsGrabbed() { return isGrabbed; }
	public bool IsBlocking() { return isBlocking; }
	
	public void SetGrabbed(bool grabbed) { isGrabbed = grabbed; }
	
	//
	private void Awake() {
		cachedBody = GetComponent<Rigidbody2D>();
		cachedHands = GetComponent<HeroHands>();
		cachedHero = GetComponent<HeroStats>();
		cachedTransform = GetComponent<Transform>();
		cachedCollision = GetComponent<HeroCollisions>();
	}
	
	private void Start() {
		if(!Camera.main) Debug.LogError("No main camera on scene");
		if(!cachedHands) Debug.LogError("Handless hero, yikes");
		
		isMoving = false;
		isGrabbed = false;
		isBlocking = false;
		
		currentJumps = 0;
	}
	
	//
	private void Update() {
		if(cachedBody == null) return;
		if(cachedHands == null) return;
		
		if(cachedHero.health == 0) return;
		
		// recovery
		if(Input.GetButtonDown(jump) && cachedHero.CanRecover()) {
			cachedHero.RestoreAfterLanding();
		}
		
		if(isGrabbed) return;
		
		// movement
		float input = Input.GetAxis(moveHorizontal);
		isMoving = Mathf.Abs(input) > Mathf.Epsilon;
		
		cachedBody.velocity = new Vector2(input * moveVelocity,cachedBody.velocity.y);
		
		// orient
		if(isMoving) cachedTransform.localScale = new Vector3(Mathf.Sign(input),1.0f,1.0f);
		
		// block
		isBlocking = Input.GetButton(block);
		
		// jump
		if(Input.GetButtonDown(jump) && !cachedCollision.InAir()) {
			cachedBody.velocity = cachedBody.velocity.WithY(jumpVelocity);
			cachedCollision.SetInAir(true);
			currentJumps = 0;
		}
		
		if(Input.GetButtonDown(jump) && cachedCollision.InAir() && (currentJumps < maxJumps)) {
			cachedBody.velocity = cachedBody.velocity.WithY(jumpVelocity);
			currentJumps++;
		}
		
		// grab
		if(isBlocking) {
			cachedHands.Drop();
		}
		else {
			if(Input.GetButtonDown(grab)) cachedHands.Grab();
			if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength,throwVertical);
		}
	}
}
