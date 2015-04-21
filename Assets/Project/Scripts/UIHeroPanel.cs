using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 */
public class UIHeroPanel : MonoBehaviour {
	
	//
	public HeroStats hero = null;
	public RectTransform hearts = null;
	public RectTransform stamina = null;
	
	public Sprite fullHeart = null;
	public Sprite emptyHeart = null;
	
	//
	private Image[] cachedHeartRenderers;
	
	//
	private void Awake() {
		cachedHeartRenderers = hearts.GetComponentsInChildren<Image>();
	}
	
	private void Update() {
		stamina.localScale = stamina.localScale.WithY(hero.stamina / hero.maxStamina);
		
		int availableHearts = (int)((cachedHeartRenderers.Length * hero.health) / (float)hero.maxHealth);
		for(int i = 0; i < cachedHeartRenderers.Length; ++i) {
			cachedHeartRenderers[i].sprite = (i < availableHearts) ? fullHeart : emptyHeart;
		}
	}
}
