using UnityEngine;
using System.Collections;

/*
 */
public class UIHeroStunbar : MonoBehaviour {
	
	//
	[Header("Data")]
	public float offset = 1.5f;
	public HeroStats hero = null;
	
	[Header("Recovery")]
	public Transform recoveryAreaStart = null;
	public Transform recoveryAreaEnd = null;
	public Transform recoverySlider = null;
	
	//
	private Transform cachedTransform;
	private SpriteRenderer[] cachedRenderers;
	
	//
	private void Awake() {
		cachedTransform = GetComponent<Transform>();
		cachedRenderers = GetComponentsInChildren<SpriteRenderer>();
	}
	
	//
	private void Update() {
		
		// placement
		cachedTransform.position = hero.transform.position + Vector3.up * offset;
		
		// visibility
		foreach(SpriteRenderer renderer in cachedRenderers) {
			renderer.enabled = hero.IsStunned();
		}
		
		// recovery circles
		recoveryAreaStart.localScale = Vector3.one * hero.recoveryAreaStart;
		recoveryAreaEnd.localScale = Vector3.one * hero.recoveryAreaEnd;
		recoverySlider.localScale = Vector3.one * hero.GetRecoveryProgress();
	}
}
