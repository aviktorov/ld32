using UnityEngine;
using System.Collections;

/*
 */
public class UIHeroStunbar : MonoBehaviour {
	
	//
	[Header("Data")]
	public float offset = 1.5f;
	public HeroStats hero = null;
	public bool debug = false;
	
	[Header("Recovery")]
	public Color defaultColor = Color.white;
	public Color recoveryColor = Color.green;
	public SpriteRenderer recoveryBackground = null;
	public Transform recoverySlider = null;
	
	//
	private Transform cachedTransform;
	private SpriteRenderer[] cachedRenderers;
	private float currentDebugTime;
	
	//
	private void Awake() {
		cachedTransform = GetComponent<Transform>();
		cachedRenderers = GetComponentsInChildren<SpriteRenderer>();
		currentDebugTime = 0.0f;
	}
	
	//
	private void Update() {
		
		// placement
		cachedTransform.position = hero.transform.position.WithZ(cachedTransform.position.z) + Vector3.up * offset;
		
		currentDebugTime += Time.deltaTime;
		while(currentDebugTime > hero.recoveryInterval) currentDebugTime -= hero.recoveryInterval;
		
		// debug
		float progress = hero.GetRecoveryProgress();
		if(debug) progress = hero.recoveryAnimation.Evaluate(Mathf.Clamp01(currentDebugTime / hero.recoveryInterval));
		
		// visibility
		if(!debug) {
			foreach(SpriteRenderer renderer in cachedRenderers) {
				renderer.enabled = hero.IsStunned();
			}
		}
		
		// recovery circles
		recoveryBackground.color = ((progress > hero.recoveryAreaStart) && (progress < hero.recoveryAreaEnd)) ? recoveryColor : defaultColor;
		recoverySlider.localScale = Vector3.one * (1.0f - progress);
	}
}
