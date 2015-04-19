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
	
	[Header("Physics")]
	public float moveVelocity = 10.0f;
	public float jumpVelocity = 2.0f;
	
	public float throwStrength = 10.0f;
	public float throwVertical = 0.2f;
	
	//
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroCollisions cachedCollision;
	private Transform cachedTransform;
	
	private bool isMoving;
	private bool isGrabbed;
	
	//
	public bool IsMoving() { return isMoving; }
	
	public bool IsGrabbed() { return isGrabbed; }
	public void SetGrabbed(bool grabbed) { isGrabbed = grabbed; }
	
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
		
		isMoving = false;
	}
	
	//
	private void Update() {
		if(cachedBody == null) return;
		if(cachedHands == null) return;
		
		if(isGrabbed) {
			if(Input.GetButtonDown(jump)) {
				// take stamina from enemy
			}
			
			return;
		}
		
		// movement
		float input = Input.GetAxis(moveHorizontal);
		isMoving = Mathf.Abs(input) > Mathf.Epsilon;
		
		cachedBody.velocity = new Vector2(input * moveVelocity,cachedBody.velocity.y);
		
		// orient
		if(isMoving) cachedTransform.localScale = new Vector3(Mathf.Sign(input),1.0f,1.0f);
		
		// jump
		if(Input.GetButtonDown(jump) && !cachedCollision.InAir()) {
			cachedBody.velocity = cachedBody.velocity.WithY(jumpVelocity);
			cachedCollision.SetInAir(true);
		}
		
		// grab
		if(Input.GetButtonDown(grab)) cachedHands.Grab();
		if(Input.GetButtonUp(grab)) cachedHands.Drop(throwStrength,throwVertical);
	}
}
