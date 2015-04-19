using UnityEngine;
using System.Collections;

/*
 */
public class HeroVisuals : MonoBehaviour {
	
	//
	[Header("Color")]
	public Color defaultColor = Color.red;
	public Color blockingColor = Color.red;
	public Color hitColor = Color.white;
	
	[Header("Visuals")]
	public float smoothness = 3.0f;
	
	//
	private SpriteRenderer cachedRenderer;
	private HeroInput cachedInput;
	
	public void OnHit() {
		cachedRenderer.color = hitColor;
	}
	
	//
	private void Awake() {
		cachedRenderer = GetComponent<SpriteRenderer>();
		cachedInput = GetComponent<HeroInput>();
	}
	
	private void Update() {
		Color targetColor = cachedInput.IsBlocking() ? blockingColor : defaultColor;
		cachedRenderer.color = Color.Lerp(cachedRenderer.color,targetColor,smoothness * Time.deltaTime);
	}
}
