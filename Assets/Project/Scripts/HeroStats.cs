using UnityEngine;
using System.Collections;

/*
 */
public class HeroStats : MonoBehaviour {
	
	//
	[Header("Stats")]
	public int maxHealth = 5;
	public int maxStamina = 100;
	
	[Header("Stamina")]
	public float staminaRegainSpeed = 10.0f;
	public float staminaCooldown = 5.0f;
	public float staminaLossMultiplier = 2.0f;
	
	[Header("Visuals")]
	public float stabilizationSmoothness = 5.0f;
	
	//
	[System.NonSerialized]
	public int health;
	
	[System.NonSerialized]
	public float stamina;
	
	//
	private float currentCooldownTime;
	
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroInput cachedInput;
	private HeroCollisions cachedCollision;
	private Transform cachedTransform;
	
	private Quaternion stableRotation;
	
	private bool stabilize;
	private bool stunned;
	
	//
	public void TakeHealth() {
		if(!stunned) return;
		
		health = Mathf.Max(0,health - 1);
		stunned = false;
	}
	
	public void TakeStamina(float amount) {
		stamina = Mathf.Max(0,stamina - Mathf.CeilToInt(amount));
	}
	
	//
	public void PrepareForGrab() {
		cachedBody.fixedAngle = false;
		cachedInput.SetGrabbed(true);
		cachedTransform.rotation = Quaternion.LookRotation(cachedTransform.forward,-Vector3.up);
		stabilize = false;
	}
	
	public void PrepareForDrop() {
		cachedCollision.SetCheckLanding(true);
		stunned = true;
	}
	
	public void RestoreAfterLanding() {
		cachedBody.fixedAngle = true;
		cachedInput.SetGrabbed(false);
		stabilize = true;
		stunned = false;
	}
	
	//
	private void Awake() {
		cachedHands = GetComponent<HeroHands>();
		cachedInput = GetComponent<HeroInput>();
		cachedCollision = GetComponent<HeroCollisions>();
		cachedBody = GetComponent<Rigidbody2D>();
		cachedTransform = GetComponent<Transform>();
		
		stabilize = true;
		stunned = false;
		stableRotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
	}
	
	private void Start() {
		health = maxHealth;
		stamina = maxStamina;
	}
	
	//
	private void Update() {
		
		if(stabilize) {
			cachedTransform.rotation = Quaternion.Slerp(
				cachedTransform.rotation,
				stableRotation,
				stabilizationSmoothness * Time.deltaTime
			);
		}
		
		// cooldown
		currentCooldownTime = Mathf.Max(0,currentCooldownTime - Time.deltaTime);
		
		// take stamina constantly if we're carrying an object in our hands
		Rigidbody2D obj = cachedHands.GetGrabbedObject();
		
		if(obj) {
			currentCooldownTime = staminaCooldown;
			stamina = Mathf.Max(0,stamina - staminaLossMultiplier * obj.mass * Time.deltaTime);
			if(Mathf.Abs(stamina) < Mathf.Epsilon) cachedHands.Drop();
		}
		
		// regain stamina
		if(Mathf.Abs(currentCooldownTime) < Mathf.Epsilon) {
			stamina = Mathf.Min(stamina + staminaRegainSpeed * Time.deltaTime,maxStamina);
		}
	}
}
