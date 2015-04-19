using UnityEngine;
using System.Collections;

/*
 */
public class UIHeroStunbar : MonoBehaviour {
	
	//
	public float offset = 1.5f;
	public HeroStats hero = null;
	
	public Transform recoveryArea = null;
	public Transform slider = null;
	
	//
	private Transform cachedTransform;
	private SpriteRenderer[] cachedRenderers;
	
	//
	private float GetParentOffset(float relativeOffset) {
		return relativeOffset / cachedTransform.localScale.x;
	}
	
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
		
		// recovery area
		float recoveryAreaWidth = hero.recoveryAreaEnd - hero.recoveryAreaStart;
		float recoveryAreaOffset = (hero.recoveryAreaEnd + hero.recoveryAreaStart) - 1.0f;
		
		recoveryArea.localScale = recoveryArea.localScale.WithX(recoveryAreaWidth);
		recoveryArea.localPosition = recoveryArea.localPosition.WithX(GetParentOffset(recoveryAreaOffset));
		
		// animate slider
		float sliderOffset = 2.0f * hero.GetRecoveryProgress() - 1.0f;
		slider.localPosition = slider.localPosition.WithX(GetParentOffset(sliderOffset));
	}
}
