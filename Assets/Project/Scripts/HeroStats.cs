using UnityEngine;
using UnityEngine.Events;

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
	
	[Header("Recovery")]
	public AnimationCurve recoveryAnimation = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
	public float recoveryInterval = 3.0f;
	public float recoveryAreaStart = 0.3f;
	public float recoveryAreaEnd = 0.5f;
	
	[Header("Visuals")]
	public float stabilizationSmoothness = 5.0f;
	
	[Header("Events")]
	public UnityEvent onDeath = null;
	
	//
	[System.NonSerialized]
	public int health;
	
	[System.NonSerialized]
	public float stamina;
	
	//
	private float currentCooldownTime;
	private float currentRecoveryTime;
	
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroInput cachedInput;
	private HeroCollisions cachedCollision;
	private Transform cachedTransform;
	
	private Quaternion stableRotation;
	
	private bool stabilize;
	private bool isStunned;
	
	//
	public bool IsStunned() { return isStunned; }
	
	//
	public float GetRecoveryProgress() {
		return recoveryAnimation.Evaluate(Mathf.Clamp01(currentRecoveryTime / recoveryInterval));
	}
	
	public bool CanRecover() {
		float progress = GetRecoveryProgress();
		return (progress > recoveryAreaStart) && (progress < recoveryAreaEnd);
	}
	
	//
	public void TakeHealth() {
		if(health == 0) return;
		if(!isStunned) return;
		
		health = Mathf.Max(0,health - 1);
		isStunned = false;
		
		if(health == 0) {
			cachedBody.fixedAngle = false;
			cachedInput.SetGrabbed(true);
			stabilize = false;
			
			onDeath.Invoke();
		}
		
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
		isStunned = true;
		currentRecoveryTime = 0.0f;
	}
	
	public void RestoreAfterLanding() {
		cachedBody.fixedAngle = true;
		cachedInput.SetGrabbed(false);
		stabilize = true;
		isStunned = false;
	}
	
	public void Reset() {
		health = maxHealth;
		stamina = maxStamina;
		
		stabilize = true;
		isStunned = false;
		
		currentCooldownTime = 0.0f;
		currentRecoveryTime = 0.0f;
	}
	
	//
	private void Awake() {
		cachedHands = GetComponent<HeroHands>();
		cachedInput = GetComponent<HeroInput>();
		cachedCollision = GetComponent<HeroCollisions>();
		cachedBody = GetComponent<Rigidbody2D>();
		cachedTransform = GetComponent<Transform>();
		
		stableRotation = Quaternion.LookRotation(Vector3.forward,Vector3.up);
	}
	
	private void Start() {
		Reset();
	}
	
	//
	private void Update() {
		
		if(health == 0) return;
		
		// recovery
		if(isStunned) {
			currentRecoveryTime = Mathf.Min(currentRecoveryTime + Time.deltaTime,recoveryInterval);
		}
		
		// stabilization
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
