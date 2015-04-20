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
	public Transform recoveryAreaStart = null;
	public Transform recoveryAreaEnd = null;
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
		cachedTransform.position = hero.transform.position + Vector3.up * offset;
		
		currentDebugTime += Time.deltaTime;
		while(currentDebugTime > hero.recoveryInterval) currentDebugTime -= hero.recoveryInterval;
		
		float progress = hero.GetRecoveryProgress();
		if(debug) progress = hero.recoveryAnimation.Evaluate(Mathf.Clamp01(currentDebugTime / hero.recoveryInterval));
		
		// visibility
		if(!debug) {
			foreach(SpriteRenderer renderer in cachedRenderers) {
				renderer.enabled = hero.IsStunned();
			}
		}
		
		// recovery circles
		recoveryAreaStart.localScale = Vector3.one * hero.recoveryAreaStart;
		recoveryAreaEnd.localScale = Vector3.one * hero.recoveryAreaEnd;
		recoverySlider.localScale = Vector3.one * (1.0f - progress);
	}
}
