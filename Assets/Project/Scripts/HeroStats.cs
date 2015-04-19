using UnityEngine;
using System.Collections;

/*
 */
public class HeroStats : MonoBehaviour {
	
	//
	[Header("Stats")]
	public int maxHealth = 100;
	public int maxStamina = 100;
	
	[Header("Stamina")]
	public float staminaRegainSpeed = 10.0f;
	public float staminaCooldown = 5.0f;
	public float staminaLossMultiplier = 2.0f;
	
	[Header("Visuals")]
	public float stabilizationSmoothness = 5.0f;
	
	//
	private int health;
	private float stamina;
	private float currentCooldownTime;
	
	private Rigidbody2D cachedBody;
	private HeroHands cachedHands;
	private HeroInput cachedInput;
	private Transform cachedTransform;
	
	private Quaternion stableRotation;
	
	private bool stabilize;
	
	//
	public void TakeHealth(float amount) {
		health = Mathf.Max(0,health - Mathf.CeilToInt(amount));
	}
	
	public void TakeStamina(float amount) {
		stamina = Mathf.Max(0,stamina - Mathf.CeilToInt(amount));
	}
	
	//
	public void PrepareForGrab() {
		cachedBody.fixedAngle = false;
		cachedInput.enabled = false;
		cachedTransform.rotation = Quaternion.LookRotation(cachedTransform.forward,-Vector3.up);
		stabilize = false;
	}
	
	public void RestoreAfterLanding() {
		cachedBody.fixedAngle = true;
		cachedInput.enabled = true;
		stabilize = true;
	}
	
	//
	private void Awake() {
		cachedHands = GetComponent<HeroHands>();
		cachedInput = GetComponent<HeroInput>();
		cachedBody = GetComponent<Rigidbody2D>();
		cachedTransform = GetComponent<Transform>();
		
		stabilize = true;
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
