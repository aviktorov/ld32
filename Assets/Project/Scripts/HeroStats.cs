using UnityEngine;
using System.Collections;

/*
 */
public class HeroStats : MonoBehaviour {
	
	//
	public int health = 100;
	public int stamina = 100;
	public float staminaRegainSpeed = 10.0f;
	public float staminaCooldown = 5.0f;
	public float staminaLossMultiplier = 2.0f;
	
	public float currentStamina;
	public int currentHealth;
	
	//
	private float currentCooldownTime;
	
	private HeroHands cachedHands;
	
	//
	public void TakeHealth(float amount) {
		currentHealth = Mathf.Max(0,currentHealth - Mathf.CeilToInt(amount));
		Debug.Log(string.Format("Ouch! Health: {0}",currentHealth));
	}
	
	public void TakeStamina(float amount) {
		currentStamina = Mathf.Max(0,currentStamina - Mathf.CeilToInt(amount));
		Debug.Log(string.Format("Getting tired? Stamina: {0}",currentStamina));
	}
	
	//
	private void Awake() {
		cachedHands = GetComponentInChildren<HeroHands>();
	}
	
	private void Start() {
		currentHealth = health;
		currentStamina = stamina;
	}
	
	//
	private void Update() {
		
		currentCooldownTime = Mathf.Max(0,currentCooldownTime - Time.deltaTime);
		
		// take stamina constantly if we're carrying an object in our hands
		Rigidbody obj = cachedHands.GetGrabbedObject();
		
		if(obj) {
			currentCooldownTime = staminaCooldown;
			currentStamina = Mathf.Max(0,currentStamina - staminaLossMultiplier * obj.mass * Time.deltaTime);
			if(Mathf.Abs(currentStamina) < Mathf.Epsilon) cachedHands.Drop();
		}
		
		// regain stamina
		if(Mathf.Abs(currentCooldownTime) < Mathf.Epsilon) {
			currentStamina = Mathf.Min(currentStamina + staminaRegainSpeed * Time.deltaTime,stamina);
		}
	}
}
